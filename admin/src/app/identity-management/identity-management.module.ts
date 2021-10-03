import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IdentityManagementRoutingModule } from './identity-management-routing.module';
import { RolesComponent } from './components/roles/roles.component';
import { RolesAddUpdateComponent } from './components/roles-add-update/roles-add-update.component';
import { UsersComponent } from './components/users/users.component';
import { UsersAddUpdateComponent } from './components/users-add-update/users-add-update.component';
import { MaterialModule } from '../shared/material.module';
import { ManagePermissionsComponent } from './components/manage-permissions/manage-permissions.component';
import { SharedModule } from '../shared/shared.module';
import { EventsComponent } from './components/events/events.component';

import { EventCategoryAddUpdateComponent } from './components/event-category-add-update/event-category-add-update.component';
import { EventCategoryComponent } from './components/event-category/event-category.component';

@NgModule({
  declarations: [RolesComponent, RolesAddUpdateComponent, UsersComponent, UsersAddUpdateComponent, ManagePermissionsComponent, EventsComponent, EventCategoryAddUpdateComponent, EventCategoryComponent],
  imports: [
    CommonModule,
    IdentityManagementRoutingModule,
    SharedModule,
    MaterialModule
  ]
})
export class IdentityManagementModule { }
