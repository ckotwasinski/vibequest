export class EmailHistoryDto {
    toEmailAddress: string;
    name: string;
    fromEmailAddress: string;
    cCEmailAddress: string;
    bCCEmailAddress: string;
    subject: string;
    body: string;
    sentOn: string;
    sentBy: number;
    isSent: boolean;
    isActive: boolean;
    lastModificationTime?: string;
    lastModifierId?: string;
    creationTime: string;
    creatorId?: string;
    id: string;  
  }
  