import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { UsersDto } from '../../models/users-dto';
import { IdentityManagementService } from '../../services/identity-management.service';
import {ToastrService} from 'ngx-toastr';
import { PaginationRequstDto } from 'src/app/shared/models/pagination';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { UsersAddUpdateComponent } from '../users-add-update/users-add-update.component';
import { MatSort, Sort } from '@angular/material/sort';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, subscribeOn } from 'rxjs/operators';
import { Policies } from 'src/app/shared/config/constants';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {

  policies = Policies;
  users: UsersDto[];
  displayedColumns: string[] = ['fullName','email','roleName','isActive','id'];
  pageSize = 10;
  pageNumber = 1;
  totalCount = 0;
  paginationRequest : PaginationRequstDto = new PaginationRequstDto();
  dataSource: MatTableDataSource<UsersDto>;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  
  pageSizeOptions: number[] = [10, 25, 100];
  loading = false;
  orderBy = "";
  order ="";
  searchText = "";
  subject = new Subject()

  constructor(private identityService: IdentityManagementService,
    private router: Router,
    private toast: ToastrService,
    public dialog: MatDialog) { }

  
  ngOnInit(): void {
    this.loading = true;
    this.populateDataSource(this.pageNumber,this.pageSize);
    this.dataSource = new MatTableDataSource(this.users);
    this.subject.pipe(
      debounceTime(500),
      distinctUntilChanged(),
      ).subscribe( (value:any) => {
        this.searchText = value;
        this.populateDataSource(1, this.paginator.pageSize,true);
        }
      );
  }

  setPageSizeOptions(setPageSizeOptionsInput: string) {
    if (setPageSizeOptionsInput) {
      this.pageSizeOptions = setPageSizeOptionsInput.split(',').map(str => +str);
    }
  }
  
  
  ngAfterViewInit() {
    this.paginator.page.subscribe(()=>{
      this.populateDataSource(this.paginator.pageIndex+1, this.paginator.pageSize);
    });
    this.dataSource.sort = this.sort;
  }
  
  populateDataSource(pageNumber: number, pageSize: number,isSearch = false){
    this.paginationRequest.pageNumber = pageNumber;
    this.paginationRequest.pageSize = pageSize;
    this.paginationRequest.order = this.order;
    this.paginationRequest.orderBy = this.orderBy;
    this.paginationRequest.filter = this.searchText
    this.identityService.getUsersList(this.paginationRequest).subscribe( 
      (data) => {
        this.users = data.items;
        this.totalCount = data.totalCount; 
        this.dataSource = new MatTableDataSource(this.users);
        this.loading = false;
        if(isSearch)
        {
          this.paginator.pageIndex = 0;
        }
      },
      (error) => {console.log(error);}
    );
  }
 
  deleteRecord(id: string) {
    if (confirm("Are you sure you want to delete")) {
      this.identityService.deleteUserById(id).subscribe(() => {
        this.toast.success("User Deleted Successfully", "Success!");
        this.populateDataSource(this.paginator.pageIndex+1, this.paginator.pageSize);
      });
    }
  }

  openDialog(id: string): void {
    const dialogRef = this.dialog.open(UsersAddUpdateComponent, {
      width: '900px',
      disableClose: true,
      data: { id : id }
    });
    dialogRef.afterClosed().subscribe(result => {
      this.populateDataSource(this.paginator.pageIndex+1, this.paginator.pageSize);
    });
    
  }

  sortData(sort: Sort) {
    this.orderBy = sort.active;
    this.order = sort.direction;
    this.populateDataSource(this.paginator.pageIndex+1, this.paginator.pageSize)
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.subject.next(filterValue);
  }
}
