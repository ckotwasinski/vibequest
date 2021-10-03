import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { CommonLookupDto } from '../../models/common-lookup-dto';
import { CommonLookupService } from '../../services/common-lookup.service';

@Component({
  selector: 'app-common-lookup-add-update',
  templateUrl: './common-lookup-add-update.component.html',
  styleUrls: ['./common-lookup-add-update.component.scss']
})
export class CommonLookupAddUpdateComponent implements OnInit {

  commonLookupForm: FormGroup;
  selectedCommonLookup = {} as any;
  id: string;
  isValidateError = false;
  loading = true;
  validation_messages = {
    'configName': [
      { type: 'required', message: 'Config Name is required.' },
      { type: 'maxlength', message: 'Config Name should be less than 100 characters.' }
    ],
    'configKey': [
      { type: 'required', message: 'Config Key is required.' },
      { type: 'maxlength', message: 'Config Key should be less than 100 characters.' }
    ],
    'configValue': [
      { type: 'required', message: 'Config Value is required.' },
      { type: 'maxlength', message: 'Config Value should be less than 100 characters.' }
    ],
    'displayOrder': [
      { type: 'required', message: 'Display Order is required.' },
      { type: 'pattern', message: 'Invalid Display Order.' }
    ],
  };
  constructor(
    public dialogRef: MatDialogRef<CommonLookupAddUpdateComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private fb: FormBuilder,
    private toast: ToastrService,
    private lookupService: CommonLookupService
    ) { 
      if (data.lookup) {
        this.selectedCommonLookup = data.lookup;
      }
    }
  ngOnInit(): void {
    if (!this.selectedCommonLookup.id) {
      this.createNew();
    }
    this.buildForm();
    this.loading = false;
  }

  createNew() {
    this.selectedCommonLookup = new CommonLookupDto();
    this.buildForm();
  }

  buildForm() {
    this.commonLookupForm = this.fb.group({
      configName: [this.selectedCommonLookup.configName || '', [Validators.required, Validators.maxLength(100)]],
      configKey: [ this.selectedCommonLookup.configKey || '', [Validators.required, Validators.maxLength(100)]],
      configValue: [ this.selectedCommonLookup.configValue || '', [Validators.required, Validators.maxLength(100)]],
      displayOrder: [ this.selectedCommonLookup.displayOrder || 0, [Validators.required,Validators.pattern("^[0-9]*$")]],
      description: [this.selectedCommonLookup.description || ''],
      isActive: [ this.selectedCommonLookup.isActive || false],
    });
  }

  saveRecord() {
    if (this.commonLookupForm.invalid) {
      this.isValidateError = true;
      return;
    }
    const createDto: CommonLookupDto = this.commonLookupForm.value;
    if (this.selectedCommonLookup.id){
      this.lookupService.updateCommonLookupById(createDto ,this.selectedCommonLookup.id).subscribe(() => {
        this.toast.success("Common Lookup updated Successfully", "Success!");
        this.dialogRef.close({
        });
      },error => {  
        this.toast.error(error.error.message, "Error!");
      })
    }else{
      this.lookupService.createCommonLookup(createDto).subscribe((data) => {
        this.toast.success("Common Lookup Inserted Successfully", "Success!");
        this.dialogRef.close({
        });
      }, (error)=>{
        this.toast.error(error.error.message, "Error!");
      });
    }
  }
}
