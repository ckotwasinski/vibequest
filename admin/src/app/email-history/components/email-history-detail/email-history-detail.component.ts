import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-email-history-detail',
  templateUrl: './email-history-detail.component.html',
  styleUrls: ['./email-history-detail.component.scss']
})
export class EmailHistoryDetailComponent implements OnInit {

  loading = true;
  constructor(public dialogRef: MatDialogRef<EmailHistoryDetailComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }
 
  ngOnInit(): void {
    this.loading = false
  }
  
}
