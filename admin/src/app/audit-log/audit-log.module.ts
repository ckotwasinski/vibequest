import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AuditLogRoutingModule } from './audit-log-routing.module';
import { SharedModule } from '../shared/shared.module';
import { MaterialModule } from '../shared/material.module';
import { ErrorLogComponent } from './components/error-log/error-log.component';
import { ActivityLogComponent } from './components/activity-log/activity-log.component';
import { AuditLogDetailsComponent } from './components/audit-log-details/audit-log-details.component';

@NgModule({
  declarations: [ErrorLogComponent, ActivityLogComponent, AuditLogDetailsComponent],
  imports: [
    CommonModule,
    SharedModule,
    MaterialModule,
    AuditLogRoutingModule
  ]
})
export class AuditLogModule { }
