import { HttpClient, HttpEventType, HttpHeaders, HttpRequest } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { RolesDto } from '../../models';
import { UsersAddUpdateDto } from '../../models/users-add-update-dto';
import { IdentityManagementService } from '../../services/identity-management.service';
import {ToastrService} from 'ngx-toastr';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-users-add-update',
  templateUrl: './users-add-update.component.html',
  styleUrls: ['./users-add-update.component.scss']
})
export class UsersAddUpdateComponent implements OnInit {

  userAddUpdateForm: FormGroup;
  selectedUser = {} as UsersAddUpdateDto;
  rolesDropdown: RolesDto[];
  id: string;
  isVisible = false;
  isEdit = false;
  selectedFile: File = null;
  isProfilePicControlVisible = true;
  isValidateError = false;
  loading = true;

  validation_messages = {
    'fullName': [
      { type: 'required', message: 'Name is required.' },
      { type: 'maxlength', message: 'Name should be less than 100 characters.' }
    ],
    'email': [
      { type: 'required', message: 'Email is required.' },
      { type: 'email', message: 'Email is invalid.' },
      { type: 'maxlength', message: 'Email should be less than 150 characters.' }
    ],
    'password': [
      { type: 'required', message: 'Password is required.' },
      { type: 'maxlength', message: 'Password should be less than 150 characters.' }
    ],
    'roleCode': [
      { type: 'required', message: 'Role is required.' },
    ]
  };

  constructor(private router: Router,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private identityService: IdentityManagementService,
    private http: HttpClient,
    private toast: ToastrService,
    public dialogRef: MatDialogRef<UsersAddUpdateComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
      if (data.id) {
        this.id = data.id;
      }
  }

  ngOnInit(): void {
    this.fillRolesDropdown();
    if (this.id) {
      this.isEdit = true;
      this.editRecord(this.id);
    }
    else {
      this.isEdit = false;
      this.loading = false;
      this.createNew();
    }
  }
  fillRolesDropdown(): void {
    this.identityService.getRoleListDropdown().subscribe(
      (data) => { this.rolesDropdown = data; },
      (error) => { console.log(error); }
    );
  }
  createNew() {
    this.selectedUser = new UsersAddUpdateDto();
    this.buildForm();
    this.isVisible = true;
  }
  buildForm() {
    this.userAddUpdateForm = this.fb.group({
      fullName: [this.selectedUser.fullName || '', [Validators.required, Validators.maxLength(100)]],
      email: [this.selectedUser.email || '', [Validators.required, Validators.email, Validators.maxLength(150)]],
      password: [this.selectedUser.password || '', [Validators.required, Validators.maxLength(150)]],
      isActive: [this.selectedUser.isActive || false],
      volunteer: [this.selectedUser.volunteer || false],
      roleCode: [this.selectedUser.roleCode || '', [Validators.required]],
      profilePhoto: [this.selectedUser.profilePhoto || '']
    });
    if (this.isEdit) {
      this.userAddUpdateForm.get('password').disable();
    }
  }
  editRecord(id: string) {
    this.identityService.getUserById(id).subscribe((data) => {
      this.selectedUser = data;
      this.buildForm();
      this.loading = false
      this.isVisible = true;
      if (this.selectedUser.profilePhoto) {
        this.isProfilePicControlVisible = false;
      }
    });
  }
  saveRecord() {
    if (this.userAddUpdateForm.invalid) {
      this.isValidateError = true;
      return;
    }
    const createUpdateUserDto: UsersAddUpdateDto = this.userAddUpdateForm.value;
    if (this.selectedUser.id) {
      var ProfilePicUpdate = '';
      if(this.selectedFile)
      {
        ProfilePicUpdate = this.selectedFile.name;
      }
      else
      {     
         ProfilePicUpdate = this.selectedUser.profilePhoto;    
      }
      createUpdateUserDto.profilePhoto = ProfilePicUpdate;
      this.identityService
        .updateUserByIdAndInput(createUpdateUserDto, this.selectedUser.id)
        .subscribe((data) => {
          if (this.selectedFile) {
            this.uploadProfilePicImage(data.id);
          }
          this.toast.success("User Updated Successfully", "Success!");
          this.dialogRef.close({
          });
        },error => {  
          this.toast.error(error.error.message, "Error!");
        });
    }
    else {
      this.identityService.createUserByInput(createUpdateUserDto).subscribe((data) => {
        if (this.selectedFile) {
          this.uploadProfilePicImage(data.id);
        }
        this.toast.success("User Inserted Successfully", "Success!");
        this.dialogRef.close({
        });
      },error => {  
        this.toast.error(error.error.message, "Error!"); 
      });
    }
  }
  onSelectFile(fileInput: any) {
    this.selectedFile = <File>fileInput.target.files[0];
    if (fileInput.target.files && fileInput.target.files[0]) {
      var reader = new FileReader();
      reader.readAsDataURL(fileInput.target.files[0]); // read file as data url
      reader.onload = (event) => { // called once readAsDataURL is completed
        this.selectedUser.profilePhotoFullPath = <string>event.target.result;
        this.isProfilePicControlVisible = false;
      }
    }
  }
  uploadProfilePicImage(id: string) {
    const formData = new FormData();
    formData.append(this.selectedFile.name, this.selectedFile);
    this.identityService.uploadProfilePhoto(formData,id).subscribe();
  
  }
  deleteProfilePic() {
    this.userAddUpdateForm.get('profilePhoto').setValue('');
    this.selectedFile = null;
    this.isProfilePicControlVisible = true;
    this.selectedUser.profilePhoto = '';
  }
}