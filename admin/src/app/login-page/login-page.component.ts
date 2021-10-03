import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators} from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { PermissionsService } from '../permission-management/services/permissions.service';
import { LoginDto } from './models/login-dto';
import { AccountService } from './service/account.service';
const { maxLength, required } = Validators;

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss']
})
export class LoginPageComponent implements OnInit {

  form: FormGroup;
  isLoggedIn = false;
  isValidateError = false;

  validation_messages = {
    'email': [
      { type: 'required', message: 'Email is required.' },
      { type: 'email', message: 'Email is invalid.' }
    ],
    'password': [
      { type: 'required', message: 'Password is required.' }
    ]
  };

  constructor(private fb: FormBuilder,
      private accountService: AccountService,
      private router: Router,
      private toast: ToastrService,
      private permissionService: PermissionsService
    ) { }

  ngOnInit(): void {
    this.buildForm();
  }

  buildForm() {
    this.form = this.fb.group({
      email: ['', [required, Validators.email, maxLength(255)]],
      password: ['', [required, maxLength(128)]],
    });
  }
  onSubmit() {
    if (this.form.invalid)
    {
      this.isValidateError = true;
      return;
    }
    let data: LoginDto = new LoginDto();
    data.Email = this.form.value.email;
    data.Password = this.form.value.password;
    this.accountService.login(data).subscribe(
      (res) => {
        this.isLoggedIn = true;
        this.permissionService.getUserPermissions().subscribe(() => {
          this.router.navigate(['/identity-management/events']);
        });
      },
      (error) => {
        this.toast.error(error.error.message, "Error!");
        console.log(error);
      }
    );
  }

  logout() {
    this.isLoggedIn = false;
    this.accountService.logout();
  }


}
