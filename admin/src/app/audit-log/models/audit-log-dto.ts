export interface AuditLogDto {
    userId?: any;
    executionTime: string;
    executionDuration: number;
    clientIpAddress: string;
    browserInfo: string;
    httpMethod: string;
    url: string;
    exception?: any;
    httpStatusCode: number;
    comments: string;
    parameters: string;
    id: string;
    userName: string;
}