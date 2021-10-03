import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { AccountService } from '../login-page/service/account.service';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { StorageKeys } from './config/constants';

@Injectable()
export class HttpIntercepterProvider implements HttpInterceptor {

  constructor(private accountService: AccountService, private route: Router) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(
      request.clone({
        setHeaders: this.getAdditionalHeaders(),
      }),
    )
      .pipe(
        catchError((error: HttpErrorResponse) => {
          if (error.status === 401) {
            this.route.navigate(['\account']);
            localStorage.removeItem(StorageKeys.user);
          }
          return throwError(error);
        })
      );
  }

  getAdditionalHeaders() {
    let headers = {};
    var user = this.accountService.getUser;
    if (user) {
      headers['Authorization'] = `Bearer ${user.accessToken}`;
    }
    return headers;
  }
}
