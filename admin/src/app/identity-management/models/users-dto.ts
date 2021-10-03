export class UsersDto {
    fullName: string;
    email: string;
    password: string;
    profilePhoto: string;
    authProvider: string;
    roleCode: string;
    roleName: string;
    volunteer: boolean;
    isActive: boolean;
    deletedBy: string;
    deletedDate: string;
    isDeleted: boolean;
    updatedDate?: string;
    updatedBy?: string;
    createdDate: string;
    createdBy?: string;
    id: string;
  }