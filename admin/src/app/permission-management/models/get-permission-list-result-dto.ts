export class GetPermissionListResultDto {
  groups: PermissionGroupDto[];
}

export class PermissionGroupDto {
  name: string;
  displayName: string;
  permissions: PermissionGrantInfoDto[];
}

export class PermissionGrantInfoDto {
  name: string;
  displayName: string;
  parentName: string;
  isGranted: boolean;
}
