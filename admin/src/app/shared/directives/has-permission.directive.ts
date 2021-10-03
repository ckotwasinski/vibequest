import { Directive, Input, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { UserPermissions } from 'src/app/permission-management/models';
import { PermissionsService } from 'src/app/permission-management/services/permissions.service';

@Directive({
  selector: '[hasPermission]'
})
export class HasPermissionDirective implements OnInit {
  private permissions: string[] = [];
  authPermissions: UserPermissions;
  private logicalOp = 'AND';
  private isHidden = true;

  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef,
    private permissionService: PermissionsService
  ) { }

  ngOnInit() {
    this.authPermissions = this.permissionService.authorizedPermissions;
    this.updateView();
  }

  @Input()
  set hasPermission(val) {
    this.permissions = val;
    this.updateView();
  }

  @Input()
  set hasPermissionOp(permop) {
    this.logicalOp = permop;
    this.updateView();
  }

  private updateView() {
    if (this.checkPermission()) {
      if(this.isHidden) {
        this.viewContainer.createEmbeddedView(this.templateRef);
        this.isHidden = false;
      }
    } else {
      this.isHidden = true;
      this.viewContainer.clear();
    }
  }

  private checkPermission() {
    let hasPermission = false;

    if (this.authPermissions && this.authPermissions.permissions) {
      for (const checkPermission of this.permissions) {
        const permissionFound = this.authPermissions.permissions.find(x => x.toUpperCase() === checkPermission.toUpperCase());

        if (permissionFound) {
          hasPermission = true;

          if (this.logicalOp === 'OR') {
            break;
          }
        } else {
          hasPermission = false;
          if (this.logicalOp === 'AND') {
            break;
          }
        }
      }
    }

    return hasPermission;
  }

  // @Input()
  // set hasPermission(permission: string) {
  //   var authPermissions = this.permissionService.authorizedPermissions;
  //   if (authPermissions && authPermissions.permissions.some(x => x == permission)) {
  //     this.viewContainer.createEmbeddedView(this.templateRef);
  //   }
  //   else {
  //     this.viewContainer.clear();
  //   }
  // }
}
