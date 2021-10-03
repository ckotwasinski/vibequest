export class UsersAddUpdateDto {
    fullName: string;
    email: string;
    password: string;
    passwordSalt: string;
    profilePhoto: string;
    authProvider: string;
    roleCode: string;
    isActive: boolean;
    volunteer: boolean;
    profilePhotoFullPath: string;
    id: string;  
  }