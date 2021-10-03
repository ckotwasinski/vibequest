import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HasPermissionDirective } from './directives/has-permission.directive';
import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { MainContentComponent } from './main-content/main-content.component';
import { HomeLayoutComponent } from './home-layout/home-layout.component';
import { RouterModule } from '@angular/router';
import { MaterialModule } from './material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    HasPermissionDirective,
    HeaderComponent,
    FooterComponent,
    MainContentComponent,
    HomeLayoutComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule
  ],
  exports: [
    HasPermissionDirective,
    FormsModule,
    ReactiveFormsModule
  ]
})
export class SharedModule { }
