import { Injectable } from '@angular/core';
import { Observable, ObservedValueOf } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { EmailTemplateDto, EmailTemplateAddUpdateDto } from '../models';
import { Pagination, PaginationRequstDto } from 'src/app/shared/models/pagination';


@Injectable({
  providedIn: 'root'
})
export class EmailTemplateService {
  apiName = environment.oAuthConfig.issuer;

  constructor(private http: HttpClient) { }

  getEmailTemplateList(request: PaginationRequstDto): Observable<Pagination<EmailTemplateDto>> {
    return this.http.request<Pagination<EmailTemplateDto>>('GET', this.apiName + '/email-template', { params: <any>request });
  }
  createEmailTemplateByInput(body: EmailTemplateAddUpdateDto): Observable<EmailTemplateDto> {
    return this.http.request<EmailTemplateDto>('POST', this.apiName + '/email-template', { body });
  }
  updateEmailTemplateByIdAndInput(body: EmailTemplateAddUpdateDto, id: string): Observable<EmailTemplateDto> {
    return this.http.request<EmailTemplateDto>('PUT', this.apiName + `/email-template/${id}`, { body });
  }
  deleteEmailTemplateById(id: string): Observable<void> {
    return this.http.request<void>('DELETE', this.apiName + `/email-template/${id}`);
  }
  getEmailTemplateById(id: string): Observable<EmailTemplateAddUpdateDto> {
    return this.http.request<EmailTemplateAddUpdateDto>('GET', this.apiName + `/email-template/${id}`);
  }
}
