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
import { EmailTemplateDto } from '../../models';
import { EmailTemplateService } from '../../services/email-template.service';
import { EmailTemplateAddUpdateComponent } from '../email-template-add-update/email-template-add-update.component';

@Component({
  selector: 'app-email-template',
  templateUrl: './email-template.component.html',
  styleUrls: ['./email-template.component.scss']
})
export class EmailTemplateComponent implements OnInit {

  policies = Policies;
  emailTemplates: EmailTemplateDto[];
  displayedColumns: string[] = ['name','templateCode','subject','isActive','id'];
  pageSize = 10;
  pageNumber = 1;
  totalCount = 0;
  paginationRequest : PaginationRequstDto = new PaginationRequstDto();
  dataSource: MatTableDataSource<EmailTemplateDto>;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  orderBy = "";
  order ="";
  pageSizeOptions: number[] = [10, 25, 100];
  searchText = "";
  subject = new Subject()

  constructor(private emailTemplateService: EmailTemplateService,
    private router: Router,
    private toast: ToastrService,
    public dialog: MatDialog) { }

  
  ngOnInit(): void {
    this.populateDataSource(this.pageNumber,this.pageSize);
    this.dataSource = new MatTableDataSource(this.emailTemplates);
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
    this.emailTemplateService.getEmailTemplateList(this.paginationRequest).subscribe( 
      (data) => {
        this.emailTemplates = data.items;
        this.totalCount = data.totalCount; 
        this.dataSource = new MatTableDataSource(this.emailTemplates);
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
      this.emailTemplateService.deleteEmailTemplateById(id).subscribe(() => {
        this.toast.success("Email Template Deleted Successfully", "Success!");
        this.populateDataSource(this.paginator.pageIndex+1, this.paginator.pageSize);
      });
    }
  }
  sortData(sort: Sort) {
    this.orderBy = sort.active;
    this.order = sort.direction;
    this.populateDataSource(this.paginator.pageIndex+1, this.paginator.pageSize)
  }


  openDialog(id: string): void {
    const dialogRef = this.dialog.open(EmailTemplateAddUpdateComponent, {
      width: '900px',
      disableClose: true,
      data: { id : id }
    });
    dialogRef.afterClosed().subscribe(result => {
      this.populateDataSource(this.paginator.pageIndex+1, this.paginator.pageSize);
    });
    
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.subject.next(filterValue);
  }

}