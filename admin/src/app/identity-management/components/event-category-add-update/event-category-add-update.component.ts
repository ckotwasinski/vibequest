import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { EventCategoryAddUpdateDto } from '../../models/event-category-add-update-dto';
import { IdentityManagementService } from '../../services/identity-management.service';

@Component({
  selector: 'app-event-category-add-update',
  templateUrl: './event-category-add-update.component.html',
  styleUrls: ['./event-category-add-update.component.scss']
})
export class EventCategoryAddUpdateComponent implements OnInit {

  id: string;
  isEdit = false;
  loading = true;
  eventCategoryAddUpdateForm: FormGroup;
  selectedEventCategory = {} as EventCategoryAddUpdateDto;
  isValidateError = false;
  isVisible = false;
  selectedFile: File = null;
  isEventPicControlVisible = true;

  validation_messages = {
    'name': [
      { type: 'required', message: 'Name is required.' },
      { type: 'maxlength', message: 'Name should be less than 100 characters.' }
    ],
    'code': [
      { type: 'required', message: 'Code is required.' },
      { type: 'maxlength', message: 'Code should be less than 15 characters.' }
    ]
  };


  constructor(private router: Router,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private identityService: IdentityManagementService,
    private http: HttpClient,
    private toast: ToastrService,
    public dialogRef: MatDialogRef<EventCategoryAddUpdateComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
      if (data.id) {
        this.id = data.id;
      }
     }

  ngOnInit(): void {
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
  createNew() {
    this.selectedEventCategory = new EventCategoryAddUpdateDto();
    this.buildForm();
    this.isVisible = true;
  }
  buildForm() {
    this.eventCategoryAddUpdateForm = this.fb.group({
      name: [this.selectedEventCategory.name || '', [Validators.required, Validators.maxLength(100)]],
      code: [this.selectedEventCategory.code || '', [Validators.required, Validators.maxLength(15)]],
      photo: [this.selectedEventCategory.photo|| '']
    });
   
  }

  editRecord(id: string) {
    this.identityService.getEventCategoryById(id).subscribe((data) => {
      console.log(data);
      this.selectedEventCategory = data;
      this.buildForm();
      this.loading = false
      this.isVisible = true;
      if (this.selectedEventCategory.photo) {
        this.isEventPicControlVisible = false;
      }
    });
  }
  saveRecord() {
    if (this.eventCategoryAddUpdateForm.invalid) {
      this.isValidateError = true;
      return;
    }
    const createUpdateEventCategoryDto: EventCategoryAddUpdateDto = this.eventCategoryAddUpdateForm.value;
    if (this.selectedEventCategory.id) {
      var eventPhotoUpdate = '';
      if(this.selectedFile)
      {
        eventPhotoUpdate = this.selectedFile.name;
      }
      else
      {     
        eventPhotoUpdate = this.selectedEventCategory.photo;    
      }
      createUpdateEventCategoryDto.photo = eventPhotoUpdate;
      this.identityService
        .updateEventCategoryByIdAndInput(createUpdateEventCategoryDto, this.selectedEventCategory.id)
        .subscribe((data) => {
          if (this.selectedFile) {
            this.uploadEventCategoryImage(data.id);
          }
          this.toast.success("User Updated Successfully", "Success!");
          this.dialogRef.close({
          });
        },error => {  
          this.toast.error(error.error.message, "Error!");
        });
    }
    else {
      this.identityService.createEventCategoryByInput(createUpdateEventCategoryDto).subscribe((data) => {
        if (this.selectedFile) {
          this.uploadEventCategoryImage(data.id);
        }
        this.toast.success("User Inserted Successfully", "Success!");
        this.dialogRef.close({
        });
      },error => {  
        this.toast.error(error.error.message, "Error!"); 
      });
    }
  }
  uploadEventCategoryImage(id: string) {
    const formData = new FormData();
    formData.append(this.selectedFile.name, this.selectedFile);
    this.identityService.uploadEventCategoryPhoto(formData,id).subscribe();
  
  }
  onSelectFile(fileInput: any) {
    this.selectedFile = <File>fileInput.target.files[0];
    if (fileInput.target.files && fileInput.target.files[0]) {
      var reader = new FileReader();
      reader.readAsDataURL(fileInput.target.files[0]); // read file as data url
      reader.onload = (event) => { // called once readAsDataURL is completed
        this.selectedEventCategory.thumbnailImagePath = <string>event.target.result;
        this.isEventPicControlVisible = false;
      }
    }
  }
  deleteCategoryPic() {
    this.eventCategoryAddUpdateForm.get('photo').setValue('');
    this.selectedFile = null;
    this.isEventPicControlVisible = true;
    this.selectedEventCategory.photo = '';
  }
}
