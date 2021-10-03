import { DeclarationListEmitMode } from "@angular/compiler";

export class EventsDto {
    Name:string;
    Description: string;
    Location: string;
    Status: string;
    EventType: string;
    Date?: string;
    StartTime?: string;
    EndTime?: string;
    deletedBy: string;
    deletedDate: string;
    isDeleted: boolean;
    updatedDate?: string;
    updatedBy?: string;
    createdDate: string;
    createdBy?: string;
    UserId: string;
    UserFullName: string;
    UserEmail: string;
}
