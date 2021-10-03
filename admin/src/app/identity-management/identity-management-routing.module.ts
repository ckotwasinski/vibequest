import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { Policies } from '../shared/config/constants';
import { AuthGuard } from '../shared/guards/auth.guard';
import { EventCategoryComponent } from './components/event-category/event-category.component';

import { EventsComponent } from './components/events/events.component';
import { ManagePermissionsComponent } from './components/manage-permissions/manage-permissions.component';
import { RolesAddUpdateComponent } from './components/roles-add-update/roles-add-update.component';

import { RolesComponent } from './components/roles/roles.component';
import { UsersAddUpdateComponent } from './components/users-add-update/users-add-update.component';
import { UsersComponent } from './components/users/users.component';

const routes: Routes = [
  
  {
    path: 'roles/manage-permission/:id',
    component: ManagePermissionsComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'roles',
    component: RolesComponent,
    data: { requiredPolicy: [Policies.role] },
    canActivate: [AuthGuard]
  },
  {
    path: 'roles-add-update',
    component: RolesAddUpdateComponent,
    data: { requiredPolicy: [Policies.roleCreate, Policies.roleEdit] },
    canActivate: [AuthGuard]
  },
  {
    path: 'users',
    component: UsersComponent,
    data: { requiredPolicy: [Policies.user] },
     canActivate: [AuthGuard]
  },
  {
    path: 'event-category',
    component: EventCategoryComponent,
    data: { requiredPolicy: [Policies.eventCategory] },
    canActivate: [AuthGuard]
  },
  {
    path: 'events',
    component: EventsComponent,
    data: { requiredPolicy: [Policies.event] },
    canActivate: [AuthGuard]
  },
  {
    path: 'users-add-update',
    component: UsersAddUpdateComponent,
    data: { requiredPolicy: [Policies.userCreate, Policies.userEdit] },
    //canActivate: [AuthGuard]
  },
  { path: '**', component: RolesComponent },
  
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class IdentityManagementRoutingModule { }
