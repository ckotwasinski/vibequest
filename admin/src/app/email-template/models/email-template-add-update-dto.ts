export class EmailTemplateAddUpdateDto {
    templateCode: string;
    name: string;
    to: string;
    cc: string;
    bcc: string;
    subject: string;
    body: string;
    isActive: boolean;
    id: string;  
  }