import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { EmailTemplateAddUpdateDto } from 'src/app/email-template/models';
import { EmailTemplateService } from 'src/app/email-template/services/email-template.service';

@Component({
  selector: 'app-email-template-add-update',
  templateUrl: './email-template-add-update.component.html',
  styleUrls: ['./email-template-add-update.component.scss']
})
export class EmailTemplateAddUpdateComponent implements OnInit {

  loading = true;
  emailTemplateAddUpdateForm: FormGroup;
  selectedEmailTemplate = {} as EmailTemplateAddUpdateDto;
  id: string;
  isValidateError = false;
  validation_messages = {
    'templateCode': [
      { type: 'required', message: 'Code is required.' },
      { type: 'maxlength', message: 'Code should be less than 50 characters.' }
    ],
    'name': [
      { type: 'required', message: 'Name is required.' },
      { type: 'maxlength', message: 'Name should be less than 100 characters.' }
    ],
    'to': [
      { type: 'email', message: 'To email is Invalid.' },
      { type: 'maxlength', message: 'To email should be less than 100 characters.' }
    ],
    'cc': [
      { type: 'email', message: 'CC email is Invalid.' },
      { type: 'maxlength', message: 'CC email should be less than 100 characters.' }
    ],
    'bcc': [
      { type: 'email', message: 'BCC email is Invalid.' },
      { type: 'maxlength', message: 'BCC email should be less than 100 characters.' }
    ],
    'subject': [
      { type: 'required', message: 'Subject is required.' },
      { type: 'maxlength', message: 'Subject should be less than 500 characters.' }
    ],
    'body': [
      { type: 'required', message: 'Body is required.' }
    ]
  };

  constructor(private router: Router,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private emailTemplateService: EmailTemplateService,
    private toast: ToastrService,
    public dialogRef: MatDialogRef<EmailTemplateAddUpdateComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
      if (data.id) {
        this.id = data.id;
      }
  }

  ngOnInit(): void {
    if (this.id) {
      this.editRecord(this.id);
    }
    else {
      this.loading = false;
      this.createNew();
    }
  }

  createNew() {
    this.selectedEmailTemplate = new EmailTemplateAddUpdateDto();
    this.buildForm();
  }
  buildForm() {
    this.emailTemplateAddUpdateForm = this.fb.group({
      templateCode: [this.selectedEmailTemplate.templateCode || '', [Validators.required, Validators.maxLength(50)]],
      name: [this.selectedEmailTemplate.name || '', [Validators.required, Validators.maxLength(100)]],
      to: [this.selectedEmailTemplate.to || '', [Validators.email,Validators.maxLength(100)]],
      cc: [this.selectedEmailTemplate.cc || '', [Validators.email,Validators.maxLength(100)]],
      bcc: [this.selectedEmailTemplate.bcc || '', [Validators.email,Validators.maxLength(100)]],
      subject: [this.selectedEmailTemplate.subject || '', [Validators.required, Validators.maxLength(500)]],
      body: [this.selectedEmailTemplate.body || '', [Validators.required]],
      isActive: [this.selectedEmailTemplate.isActive || false]
    });
  }
  editRecord(id: string) {
    this.emailTemplateService.getEmailTemplateById(id).subscribe((data) => {
      this.selectedEmailTemplate = data;
      this.loading = false;
      this.buildForm();
    });
  }

  saveRecord() {
    if (this.emailTemplateAddUpdateForm.invalid) {
      this.isValidateError = true;
      return;
    }
    const createUpdateEmailTemplateDto: EmailTemplateAddUpdateDto = this.emailTemplateAddUpdateForm.value;
    if (this.selectedEmailTemplate.id) {
      this.emailTemplateService
        .updateEmailTemplateByIdAndInput(createUpdateEmailTemplateDto, this.selectedEmailTemplate.id)
        .subscribe(() => {
          this.toast.success("Email Template Updated Successfully", "Success!");
          this.dialogRef.close({
          });
        }, error => {
          this.toast.error(error.error.message, "Error!");
        });
    }
    else {
      this.emailTemplateService.createEmailTemplateByInput(createUpdateEmailTemplateDto).subscribe(() => {
        this.toast.success("Email Template Inserted Successfully", "Success!");
        this.dialogRef.close({
        });
      }, error => {
        this.toast.error(error.error.message, "Error!");
      });
    }
  }

}
