export class UpdatePermissionsDto {
    providerKey: string;
    permissions: UpdatePermissionDto[];
  }
  
  export class UpdatePermissionDto {
    displayName : string;
    name: string;
    isGranted: boolean;
  }
  