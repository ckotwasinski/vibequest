import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Pagination, PaginationRequstDto } from 'src/app/shared/models/pagination';
import { AuditLogDto } from '../models';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuditLogService {
  apiName = environment.oAuthConfig.issuer;

  constructor(private http: HttpClient) { }

  getAuditLogList(request: PaginationRequstDto) : Observable<Pagination<AuditLogDto>> {
    return this.http.request<Pagination<AuditLogDto>>('GET', this.apiName + '/audit/activity-log', {params:<any>request});
  }

  getErrorLogList(request: PaginationRequstDto) : Observable<Pagination<AuditLogDto>> {
    return this.http.request<Pagination<AuditLogDto>>('GET', this.apiName + '/audit/error-log', {params:<any>request});
  }
}
