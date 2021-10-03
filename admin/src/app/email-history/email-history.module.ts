import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EmailHistoryRoutingModule } from './email-history-routing.module';
import { EmailHistoryComponent } from './components/email-history-list/email-history.component';
import { CKEditorModule } from 'ckeditor4-angular';
import { EmailHistoryDetailComponent } from './components/email-history-detail/email-history-detail.component';
import { SharedModule } from '../shared/shared.module';
import { MaterialModule } from '../shared/material.module';


@NgModule({
  declarations: [EmailHistoryComponent,EmailHistoryDetailComponent],
  imports: [
    CommonModule,
    EmailHistoryRoutingModule,
    SharedModule,
    CKEditorModule,
    MaterialModule
  ]
})
export class EmailHistoryModule { }
