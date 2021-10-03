import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ActivityLogComponent } from './components/activity-log/activity-log.component';
import { AuditLogDetailsComponent } from './components/audit-log-details/audit-log-details.component';
import { ErrorLogComponent } from './components/error-log/error-log.component';

const routes: Routes = [
  {
    path: 'activity',
    component: ActivityLogComponent
  },
  {
    path: 'error',
    component: ErrorLogComponent
  },
  {
    path: 'audit-log-details',
    component: AuditLogDetailsComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuditLogRoutingModule { }
