import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EmailHistoryDetailComponent } from './components/email-history-detail/email-history-detail.component';

import { EmailHistoryComponent } from './components/email-history-list/email-history.component';

const routes: Routes = [
  { path: 'email-history', component: EmailHistoryComponent },
  { path: 'email-history-detail', component: EmailHistoryDetailComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmailHistoryRoutingModule { }
