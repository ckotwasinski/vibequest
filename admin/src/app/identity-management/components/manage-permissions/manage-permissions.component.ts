import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PermissionGrantInfoDto, PermissionGroupDto, UpdatePermissionsDto } from 'src/app/permission-management/models';
import { PermissionsService } from 'src/app/permission-management/services/permissions.service';
import { ToastrService } from 'ngx-toastr';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { RolesDto } from '../../models';


@Component({
  selector: 'app-manage-permissions',
  templateUrl: './manage-permissions.component.html',
  styleUrls: ['./manage-permissions.component.scss']
})
export class ManagePermissionsComponent implements OnInit {
  groups: PermissionGroupDto[];
  permissions = {} as UpdatePermissionsDto;
  permissionsDiff : any;
  roleId: string;
  roleName: string;
  selectAllTab = false;
  loading = true;

  constructor(private permissionService: PermissionsService,
    private route: ActivatedRoute,private router: Router,private toast: ToastrService,
    public dialogRef: MatDialogRef<ManagePermissionsComponent>,
    @Inject(MAT_DIALOG_DATA) public role: RolesDto) {
      if (role) {
        this.roleId = role.id;
      }
   }

  ngOnInit(): void {
    if (this.roleId) {
      this.getPermissions();
    }
  }

  getPermissions() {
    this.permissionService.getPermissionsByProviderKey(this.roleId).subscribe((response) => {
      if (response) {
        this.groups = response.groups;
        this.permissions.providerKey = this.roleId;
        this.permissions.permissions = [];
        this.groups.map(group => {
          group.permissions.map(permission => {
            this.permissions.permissions.push({
              name: permission.name,
              isGranted: permission.isGranted,
              displayName : permission.displayName
            });
          });
        });
        this.setGrantCheckboxState();
        this.permissionsDiff =JSON.parse(JSON.stringify(this.permissions));
        this.loading = false;
      }
    });
  }

  updatePermissions() {
    this.permissionService.updatePermissions(this.permissions).subscribe((response) => {
      this.toast.success("Permissions Updated Successfully", "Success!");
      this.dialogRef.close({
      });
      this.router.navigate(['/identity-management/roles/']);
    }, error => {
      this.toast.error(error.error.message, "Error!");
    });
  }
  isSelectAllChecked(permission : PermissionGrantInfoDto[]){
    return permission.length === permission.filter(x=>x.isGranted).length;
  }
  getChecked(name: string) {
    return (this.permissions.permissions.find((per) => per.name === name) || { isGranted: false }).isGranted;
  }
  onClickSelectAll() {
    this.permissions.permissions = this.permissions.permissions.map((permission) => ({
      ...permission,
      isGranted:
        !this.selectAllTab,
    }));
    this.groups.map(group=>{
      group.permissions.map(permission=>{
        permission.isGranted = !this.selectAllTab ;
      });
    });
  }

  onClickSelectThisTab(group : PermissionGroupDto) {
    const checkboxElement = document.querySelector('#selectGroup_' + group.name) as any;

    var groups = this.groups.filter(x=>x.displayName == group.displayName)[0];
      groups.permissions.forEach((permission) => {
        permission.isGranted = checkboxElement.checked;
        const index = this.permissions.permissions.findIndex((per) => per.name === permission.name);

        this.permissions.permissions = [
          ...this.permissions.permissions.slice(0, index),
          { ...this.permissions.permissions[index], isGranted: checkboxElement.checked },
          ...this.permissions.permissions.slice(index + 1),
        ];
      });
      this.setGrantCheckboxState();
  }

  setGrantCheckboxState() {
    const selectedAllPermissions =this.permissions.permissions.filter((per) => per.isGranted);
    if (selectedAllPermissions.length === this.permissions.permissions.length) {
      this.selectAllTab = true;
    } else{
      this.selectAllTab = false;
    }
  }

  setTabCheckboxState(name : string,element : any) {
      var groups = this.groups.filter(x=>x.name == name);
      groups.map((permissions)=>{
        const selectedPermissions = permissions.permissions.filter((per) => per.isGranted);

        if (selectedPermissions.length === permissions.permissions.length) {
          element.indeterminate = false;
          element.checked = element.checked;
        } else if (selectedPermissions.length === 0) {
          element.indeterminate = false;
          element.checked = element.checked;
        } else {
          element.indeterminate = true;
        }
      })
  }
  
  submit(){
    if(this.permissions?.permissions?.length > 0){
      this.updatePermissions();
    }
  }
  isEquivalent() {
    return JSON.stringify(this.permissions.permissions) === JSON.stringify(this.permissionsDiff.permissions);
  }
  onClickCheckbox(clickedPermission: PermissionGrantInfoDto,name : string,value) {
    setTimeout(() => {
      this.groups.map((group)=>{
          group.permissions.map((permission)=>{
          if (clickedPermission.name === permission.name) {
            permission.isGranted =!permission.isGranted;
          } else if (clickedPermission.name === permission.parentName && !clickedPermission.isGranted) {
            permission.isGranted = false ;
          } else if (clickedPermission.parentName === permission.name && !clickedPermission.isGranted) {
            permission.isGranted = true ;
          }

        const index = this.permissions.permissions.findIndex((per) => per.name === permission.name);
        this.permissions.permissions[index].isGranted = permission.isGranted;
     });
    });
      this.setGrantCheckboxState();
    }, 0);
  }

  getAssignedCount(groupName) {
    return this.permissions.permissions.reduce((acc, val) => (val.name.split('.')[0] === groupName && val.isGranted ? acc + 1 : acc), 0);
  }
}
