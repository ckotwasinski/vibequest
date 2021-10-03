import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EmailTemplateRoutingModule } from './email-template-routing.module';
import { EmailTemplateComponent } from './components/email-template/email-template.component';
import { EmailTemplateAddUpdateComponent } from './components/email-template-add-update/email-template-add-update.component';
import { CKEditorModule } from 'ckeditor4-angular';
import { SharedModule } from '../shared/shared.module';
import { MaterialModule } from '../shared/material.module';

@NgModule({
  declarations: [EmailTemplateComponent, EmailTemplateAddUpdateComponent],
  imports: [
    CommonModule,
    EmailTemplateRoutingModule,
    SharedModule,
    CKEditorModule,
    MaterialModule
  ]
})
export class EmailTemplateModule { }
