import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { GetPermissionListResultDto } from '../models/get-permission-list-result-dto';
import { UpdatePermissionsDto } from '../models/update-permissions-dto';
import { UserPermissions } from '../models/user-permissions';
import { RolesDto } from 'src/app/identity-management/models';

@Injectable({
  providedIn: 'root'
})
export class PermissionsService {
  apiName = environment.oAuthConfig.issuer;
  private permissionSource = new BehaviorSubject<UserPermissions>(null);
  userPermissions$ = this.permissionSource.asObservable();

  constructor(private http: HttpClient) { }

  public get authorizedPermissions() : UserPermissions {
    return this.permissionSource.value;;
  }

  getPermissionsByProviderKey(roleId: string): Observable<GetPermissionListResultDto> {
    return this.http.request<GetPermissionListResultDto>('GET', this.apiName + `/permissions/${roleId}`);
  }
  updatePermissions(permissions: UpdatePermissionsDto): Observable<RolesDto[]> {
    return this.http.request<RolesDto[]>('POST', this.apiName + `/permissions`, { body: permissions });
  }

  getUserPermissions() {
    return this.http.request<string[]>('GET', this.apiName + `/permissions/by-user`)
    .pipe(
      map((x) => {
        var permissionSourceData = {} as UserPermissions;
        permissionSourceData.permissions = x;
        this.permissionSource.next(permissionSourceData);
      })
    );
  }

  removePermissions() {
    this.permissionSource.next(null);
  }
}
