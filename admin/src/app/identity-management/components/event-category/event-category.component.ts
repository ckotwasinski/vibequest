import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { Policies } from 'src/app/shared/config/constants';
import { PaginationRequstDto } from 'src/app/shared/models/pagination';
import { environment } from 'src/environments/environment';
import { EventCategoryDto } from '../../models/event-category-dto';
import { IdentityManagementService } from '../../services/identity-management.service';
import { EventCategoryAddUpdateComponent } from '../event-category-add-update/event-category-add-update.component';

@Component({
  selector: 'app-event-category',
  templateUrl: './event-category.component.html',
  styleUrls: ['./event-category.component.scss']
})
export class EventCategoryComponent implements OnInit {

  policies = Policies;
  eventCategory: EventCategoryDto[];
  displayedColumns: string[] = ['name','code','photo','id'];
  pageSize = 10;
  pageNumber = 1;
  totalCount = 0;
  imagePath = environment.oAuthConfig.issuer+"/images/categories/thumbnail/";
  paginationRequest : PaginationRequstDto = new PaginationRequstDto();
  dataSource: MatTableDataSource<EventCategoryDto>;
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
    this.dataSource = new MatTableDataSource(this.eventCategory);
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
    this.identityService.getEventCategoryList(this.paginationRequest).subscribe( 
      (data) => {
        this.eventCategory = data.items;
        this.totalCount = data.totalCount; 
        this.dataSource = new MatTableDataSource(this.eventCategory);
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
      this.identityService.deleteEventCategoryById(id).subscribe(() => {
        this.toast.success("User Deleted Successfully", "Success!");
        this.populateDataSource(this.paginator.pageIndex+1, this.paginator.pageSize);
      });
    }
  }

  openDialog(id: string): void {
    const dialogRef = this.dialog.open(EventCategoryAddUpdateComponent, {
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
