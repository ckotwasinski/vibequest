import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommonLookupAddUpdateComponent } from './components/common-lookup-add-update/common-lookup-add-update.component';
import { CommonLookupComponent } from './components/common-lookup/common-lookup.component';
import { SharedModule } from '../shared/shared.module';
import { CommonLookupRoutingModule } from './common-lookup-routing.module';
import { MaterialModule } from '../shared/material.module';

@NgModule({
  declarations: [CommonLookupAddUpdateComponent, CommonLookupComponent],
  imports: [
    CommonModule,
    SharedModule,
    CommonLookupRoutingModule,
    MaterialModule
  ]
})
export class CommonLookupModule { }
