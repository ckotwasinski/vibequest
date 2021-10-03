import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { PaginationRequstDto } from 'src/app/shared/models/pagination';
import { EmailHistoryDto } from '../../models/email-history-dto';
import { EmailHistoryService } from '../../services/email-history.service';
import { EmailHistoryDetailComponent } from '../email-history-detail/email-history-detail.component';

@Component({
  selector: 'app-email-history',
  templateUrl: './email-history.component.html',
  styleUrls: ['./email-history.component.scss']
})
export class EmailHistoryComponent implements OnInit {

  emailHistory: EmailHistoryDto[];
  displayedColumns: string[] = ['body','name','toEmailAddress','fromEmailAddress','subject','sentOn'];
  pageSize = 10;
  pageNumber = 1;
  totalCount = 0;
  paginationRequest : PaginationRequstDto = new PaginationRequstDto();
  dataSource: MatTableDataSource<EmailHistoryDto>;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  pageSizeOptions: number[] = [10, 25, 100];
  loading = false;
  @ViewChild(MatSort) sort: MatSort;
  orderBy = "";
  order ="";
  searchText = "";
  subject = new Subject()

  constructor(private emailHistoryService: EmailHistoryService,
    private router: Router,
    public dialog: MatDialog) { }

  
  ngOnInit(): void {
    this.loading = true;
    this.populateDataSource(this.pageNumber,this.pageSize);
    this.dataSource = new MatTableDataSource(this.emailHistory);
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
    this.emailHistoryService.getEmailHistoryList(this.paginationRequest).subscribe( 
      (data) => {
        this.emailHistory = data.items;
        this.totalCount = data.totalCount; 
        this.dataSource = new MatTableDataSource(this.emailHistory);
        this.loading = false;
        if(isSearch)
        {
          this.paginator.pageIndex = 0;
        }
      },
      (error) => {console.log(error);}
    );
  }
 
  openDialog(body: string,name: string): void {
    const dialogRef = this.dialog.open(EmailHistoryDetailComponent, {
      width: '900px',
      data: { content: body,title: name }
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
