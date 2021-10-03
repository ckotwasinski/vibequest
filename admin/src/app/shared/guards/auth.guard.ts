import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AccountService } from 'src/app/login-page/service/account.service';
import { PermissionsService } from 'src/app/permission-management/services/permissions.service';
import { StorageKeys } from '../config/constants';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private router: Router,
    private accountService: AccountService,
    private permissionService: PermissionsService) {

  }
  canActivate(next: ActivatedRouteSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    if (localStorage.getItem(StorageKeys.user) != null) {
      var authPermissions = this.permissionService.authorizedPermissions;
      if (!authPermissions || authPermissions.permissions.length == 0) {
        this.accountService.logout();
        return false;
      }
      this.accountService.setUser();
      let requiredPolicy = next.data["requiredPolicy"];

      if (requiredPolicy) {
        var authPermissions = this.permissionService.authorizedPermissions;
        if (authPermissions && authPermissions.permissions.some(x => requiredPolicy.some((y:any) => x == y))) {
          return true;
        }
        else {
          this.router.navigate(['/unauthorized']);
          return false;
        }
      }
      else {
        return true;
      }
    } else {
      this.router.navigate(['/account']);
      return false;
    }
  }

}
