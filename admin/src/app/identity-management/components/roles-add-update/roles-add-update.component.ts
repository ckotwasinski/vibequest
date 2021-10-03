import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { RolesAddUpdateDto, RolesDto } from '../../models';
import { IdentityManagementService } from '../../services/identity-management.service';
import { ToastrService } from 'ngx-toastr';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-roles-add-update',
  templateUrl: './roles-add-update.component.html',
  styleUrls: ['./roles-add-update.component.scss']
})
export class RolesAddUpdateComponent implements OnInit {

  rolesAddUpdateForm: FormGroup;
  selectedRole = {} as RolesAddUpdateDto;
  id: string;
  isValidateError = false;
  loading = true;

  validation_messages = {
    'name': [
      { type: 'required', message: 'Name is required.' },
      { type: 'maxlength', message: 'Name should be less than 50 characters.' }
    ],
    'code': [
      { type: 'required', message: 'Code is required.' },
      { type: 'maxlength', message: 'Code should be less than 50 characters.' }
    ]
  };

  constructor(private router: Router,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private identityService: IdentityManagementService,
    private toast: ToastrService,
    public dialogRef: MatDialogRef<RolesAddUpdateComponent>,
    @Inject(MAT_DIALOG_DATA) public data: RolesDto) {
    if (data) {
      this.selectedRole = data;
    }
  }

  ngOnInit(): void {
    if (this.selectedRole) {
      this.buildForm();
    }
    else {
      this.createNew();
    }
    this.loading = false;
  }

  createNew() {
    this.selectedRole = new RolesAddUpdateDto();
    this.buildForm();
  }
  buildForm() {
    this.rolesAddUpdateForm = this.fb.group({
      name: [this.selectedRole.name || '', [Validators.required, Validators.maxLength(50)]],
      code: [this.selectedRole.code || '', [Validators.required, Validators.maxLength(50)]]
    });
  }
  saveRecord() {
    if (this.rolesAddUpdateForm.invalid) {
      this.isValidateError = true;
      return;
    }
    const createUpdateRoleDto: RolesAddUpdateDto = this.rolesAddUpdateForm.value;
    if (this.selectedRole.id) {
      this.identityService
        .updateRoleByIdAndInput(createUpdateRoleDto, this.selectedRole.id)
        .subscribe(() => {
          this.toast.success("Role Updated Successfully", "Success!");
          this.dialogRef.close({
          });
        }, error => {
          this.toast.error(error.error.message, "Error!");
        });
    }
    else {
      this.identityService.createRoleByInput(createUpdateRoleDto).subscribe(() => {
        this.toast.success("Role Inserted Successfully", "Success!");
        this.dialogRef.close({
        });
      }, error => {
        this.toast.error(error.error.message, "Error!");
      });
    }
  }

}
