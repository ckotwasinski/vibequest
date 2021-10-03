import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, ReplaySubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { LoginDto } from '../models/login-dto';
import { map } from 'rxjs/operators';
import { User } from '../models/user';
import { Router } from '@angular/router';
import { StorageKeys } from 'src/app/shared/config/constants';
import { PermissionsService } from 'src/app/permission-management/services/permissions.service';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  apiName = environment.oAuthConfig.issuer;
  private currentUserSource = new BehaviorSubject<User>(null);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient,
    private route: Router,
    private permissionService: PermissionsService) { }

  public get getUser() : User {
    return this.currentUserSource.value;;
  }

  login(data: LoginDto) {
    return this.http.request('POST', this.apiName + '/account/login', {body:data}).pipe(
      map((response: User) => {
        const user = response;
        if(user){
          localStorage.setItem(StorageKeys.user,JSON.stringify(user));
          this.setCurrentUser(user);
        }
      })
    );
  }

  setCurrentUser(user: User){
    this.currentUserSource.next(user);
  }
  logout() {
    localStorage.removeItem(StorageKeys.user);
    localStorage.removeItem(StorageKeys.selectedPanel);
    this.currentUserSource.next(null);
    this.permissionService.removePermissions();
    this.route.navigate(['/account']);
  }

  setUser(){
    this.currentUserSource.next(JSON.parse(localStorage.getItem(StorageKeys.user)));
    this.currentUser$ = this.currentUserSource.asObservable();
  }
}
