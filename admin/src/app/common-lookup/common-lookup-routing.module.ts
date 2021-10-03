import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CommonLookupComponent } from './components/common-lookup/common-lookup.component';


const routes: Routes = [
  { 
    path: '',
    component: CommonLookupComponent 
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CommonLookupRoutingModule { }
