export class EmailTemplateDto {
    templateCode: string;
    name: string;
    to: string;
    cc: string;
    bcc: string;
    subject: string;
    body: string;
    isActive: boolean;
    lastModificationTime?: string;
    lastModifierId?: string;
    creationTime: string;
    creatorId?: string;
    id: string;  
  }
  