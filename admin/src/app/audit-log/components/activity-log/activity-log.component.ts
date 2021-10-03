import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { PaginationRequstDto } from 'src/app/shared/models/pagination';
import { AuditLogDto } from '../../models';
import { AuditLogService } from '../../services/audit-log.service';
import { MatPaginator } from '@angular/material/paginator';
import {MatSort, Sort} from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { MatDialog } from '@angular/material/dialog';
import { AuditLogDetailsComponent } from '../audit-log-details/audit-log-details.component';


@Component({
  selector: 'app-activity-log',
  templateUrl: './activity-log.component.html',
  styleUrls: ['./activity-log.component.scss']
})

export class ActivityLogComponent implements AfterViewInit, OnInit {
  displayedColumns: string[] = ['id','userName','executionTime','clientIpAddress',
  'httpMethod','url','httpStatusCode'];
  activityLogs: AuditLogDto[];
  pageSize = 10;
  pageNumber = 1;
  totalCount = 0;
  paginationRequest : PaginationRequstDto = new PaginationRequstDto();
  dataSource: MatTableDataSource<AuditLogDto>;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  pageSizeOptions: number[] = [10, 25, 100];
  loading = false;
  orderBy = "";
  order ="";
  searchText = "";
  subject = new Subject()

  constructor(private auditLogService: AuditLogService,
    private dialog: MatDialog) { }
  
  ngOnInit(): void {
    this.loading = true;
    this.populateDataSource(this.pageNumber,this.pageSize);
    this.dataSource = new MatTableDataSource(this.activityLogs);
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
    this.paginationRequest.filter = this.searchText;
    this.auditLogService.getAuditLogList(this.paginationRequest).subscribe( 
      (data) => {
        this.activityLogs = data.items;
        this.totalCount = data.totalCount; 
        this.dataSource = new MatTableDataSource(this.activityLogs);
        this.loading = false;
        if(isSearch)
        {
          this.paginator.pageIndex = 0;
        }
      },
      (error) => {console.log(error);}
    );
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

  openDialog(id: string,logData: any): void {
    const dialogRef = this.dialog.open(AuditLogDetailsComponent, {
      width: '1200px',
      data: { id: id , logData: logData , isErrorLog: false}
    });

  }
}
