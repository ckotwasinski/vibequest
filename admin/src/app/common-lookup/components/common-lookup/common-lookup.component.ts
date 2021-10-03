import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { PaginationRequstDto } from 'src/app/shared/models/pagination';
import { CommonLookupDto } from '../../models/common-lookup-dto';
import { CommonLookupService } from '../../services/common-lookup.service';
import { CommonLookupAddUpdateComponent } from '../common-lookup-add-update/common-lookup-add-update.component';
import { Policies } from '../../../shared/config/constants';

@Component({
  selector: 'app-common-lookup',
  templateUrl: './common-lookup.component.html',
  styleUrls: ['./common-lookup.component.scss']
})
export class CommonLookupComponent implements  AfterViewInit, OnInit {
  policies = Policies;
  lookups: CommonLookupDto[];
  displayedColumns: string[] = ['configName','configKey','configValue','id'];
  pageSize = 10;
  pageNumber = 1;
  totalCount = 10;
  length = 10;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  paginationRequest : PaginationRequstDto = new PaginationRequstDto();
  dataSource: MatTableDataSource<CommonLookupDto>;
  loading = false;
  pageSizeOptions: number[] = [10, 25, 100];
  orderBy = "";
  order ="";
  searchText = "";
  subject = new Subject()

  constructor(public dialog: MatDialog,
    private lookupService: CommonLookupService,
    private toast: ToastrService,
    ) { }

  ngAfterViewInit() {
    this.paginator.page.subscribe(()=>{
      this.populateDataSource(this.paginator.pageIndex+1, this.paginator.pageSize);
    });
    this.dataSource.sort = this.sort;
  }
  ngOnInit(): void {
    this.loading = true;
    this.populateDataSource(this.pageNumber,this.pageSize);
    this.dataSource = new MatTableDataSource(this.lookups);
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

  populateDataSource(pageNumber: number, pageSize: number,isSearch = false){
    this.paginationRequest.pageNumber = pageNumber;
    this.paginationRequest.pageSize = pageSize;
    this.paginationRequest.order = this.order;
    this.paginationRequest.orderBy = this.orderBy;
    this.paginationRequest.filter = this.searchText;
    this.lookupService.getCommonLookupList(this.paginationRequest).subscribe(
      (data) => {
        this.lookups = data.items;
        this.totalCount = data.totalCount;
        this.dataSource = new MatTableDataSource(this.lookups);
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
      this.lookupService.deleteCommonLookupById(id).subscribe(() => {
        this.toast.success("Common Lookup  Deleted Successfully", "Success!");
        this.populateDataSource(this.paginator.pageIndex+1, this.paginator.pageSize);
      });
    }
  }

  openDialog(cl: CommonLookupDto): void {
    const dialogRef = this.dialog.open(CommonLookupAddUpdateComponent, {
      width: '900px',
      disableClose: true,
      data: { lookup : cl}
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




