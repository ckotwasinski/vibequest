import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EmailTemplateAddUpdateComponent } from './components/email-template-add-update/email-template-add-update.component';

import { EmailTemplateComponent } from './components/email-template/email-template.component';

const routes: Routes = [
  { 
    path: 'email-template',
   component: EmailTemplateComponent 
  },
  {
    path: 'email-template-add-update',
    component: EmailTemplateAddUpdateComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmailTemplateRoutingModule { }
