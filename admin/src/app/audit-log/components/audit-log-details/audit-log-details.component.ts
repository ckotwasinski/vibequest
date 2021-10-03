import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { PaginationRequstDto } from 'src/app/shared/models/pagination';
import { AuditLogDto } from '../../models';

@Component({
  selector: 'app-audit-log-details',
  templateUrl: './audit-log-details.component.html',
  styleUrls: ['./audit-log-details.component.scss']
})
export class AuditLogDetailsComponent implements OnInit {

  displayedColumns: string[] = ['executionDuration', 'browserInfo', 'parameters', 'comments'];
  pageSize = 10;
  pageNumber = 1;
  totalCount = 0;
  paginationRequest: PaginationRequstDto = new PaginationRequstDto();
  dataSource: MatTableDataSource<AuditLogDto>;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  pageSizeOptions: number[] = [10, 25, 100];
  loading = false;
  orderBy = "";
  order = "";

  id: string;
  logData: AuditLogDto[] = [];
  isErrorLog = false;
  constructor(public dialogRef: MatDialogRef<AuditLogDetailsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    if (data) {
      this.id = data.id;
      this.isErrorLog = data.isErrorLog;
      this.logData[0] = data.logData;
      if (this.isErrorLog) {
        this.displayedColumns = ['executionDuration', 'browserInfo', 'exception', 'parameters', 'comments'];
      }
    }
  }

  ngOnInit(): void {
    this.loading = true;
    this.populateDataSource(this.pageNumber, this.pageSize);
    this.dataSource = new MatTableDataSource(this.logData);
  }


  setPageSizeOptions(setPageSizeOptionsInput: string) {
    if (setPageSizeOptionsInput) {
      this.pageSizeOptions = setPageSizeOptionsInput.split(',').map(str => +str);
    }
  }


  ngAfterViewInit() {
    this.paginator.page.subscribe(() => {
      this.populateDataSource(this.paginator.pageIndex + 1, this.paginator.pageSize);
    });
    this.dataSource.sort = this.sort;
  }

  populateDataSource(pageNumber: number, pageSize: number) {
    this.paginationRequest.pageNumber = pageNumber;
    this.paginationRequest.pageSize = pageSize;
    this.paginationRequest.order = this.order;
    this.paginationRequest.orderBy = this.orderBy;
    this.totalCount = this.logData.length;
    this.dataSource = new MatTableDataSource(this.logData);
    this.loading = false;
  }

}
