import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomePageComponent } from './home-page/home-page.component';
import { Policies } from './shared/config/constants';
import { AuthGuard } from './shared/guards/auth.guard';
import { HomeLayoutComponent } from './shared/home-layout/home-layout.component';
import { UnauthorizedComponent } from './unauthorized/unauthorized.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: '/home',
    pathMatch: 'full',
  },
  {
    path: 'home',
    component: HomeLayoutComponent,
    loadChildren: () => import('./home-page/home-page.module').then(m => m.HomePageModule),
     canActivate: [AuthGuard]
  },
  // {
  //   path: 'home-page',
  //   component: HomePageComponent,
  //   loadChildren: () => import('./home-page/home-page.module').then(m => m.HomePageModule),
  //   canActivate: [AuthGuard]
  // },

  {
    path: 'account',
    loadChildren: () => import('./login-page/login-page.module').then(m => m.LoginPageModule)
  },
  {
    path: 'identity-management',
    component: HomeLayoutComponent,
    loadChildren: () => import('./identity-management/identity-management.module').then(m => m.IdentityManagementModule),
    canActivate: [AuthGuard]
  },
  {
    path: 'email-template',
    component: HomeLayoutComponent,
    loadChildren: () => import('./email-template/email-template.module').then(m => m.EmailTemplateModule),
    canActivate: [AuthGuard]
  },
  {
    path: 'email-history',
    component: HomeLayoutComponent,
    loadChildren: () => import('./email-history/email-history.module').then(m => m.EmailHistoryModule),
     canActivate: [AuthGuard]
  },
  {
    path: 'common-lookup',
    component: HomeLayoutComponent,
    loadChildren: () => import('./common-lookup/common-lookup.module').then(m => m.CommonLookupModule),
     canActivate: [AuthGuard]
  },
 
  {
    path: 'unauthorized',
    component: UnauthorizedComponent
  },
  {
    path: 'audit',
    component: HomeLayoutComponent,
    loadChildren: () => import('./audit-log/audit-log.module').then(m => m.AuditLogModule),
    canActivate: [AuthGuard],
    data: { requiredPolicy: [Policies.auditLog] }
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
