import { Injectable } from '@angular/core';
import { Observable, ObservedValueOf } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { EmailHistoryDto } from '../models/email-history-dto';
import { Pagination, PaginationRequstDto } from 'src/app/shared/models/pagination';

@Injectable({
  providedIn: 'root'
})
export class EmailHistoryService {
  apiName = environment.oAuthConfig.issuer;

  constructor(private http: HttpClient) { }

  getEmailHistoryList(request: PaginationRequstDto):  Observable<Pagination<EmailHistoryDto>> {
    return this.http.request<Pagination<EmailHistoryDto>>('GET', this.apiName + '/email-history', {params:<any>request});
  }
  
}
