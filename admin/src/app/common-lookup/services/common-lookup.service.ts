import { Injectable } from '@angular/core';
import { Pagination, PaginationRequstDto } from 'src/app/shared/models/pagination';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { CommonLookupDto } from '../models/common-lookup-dto';

@Injectable({
  providedIn: 'root'
})
export class CommonLookupService {
  apiName = environment.oAuthConfig.issuer;

  ctheader = new HttpHeaders;

  constructor(private http: HttpClient) { }

  getCommonLookupList(req: PaginationRequstDto): Observable<Pagination<CommonLookupDto>> {
    return this.http.request<Pagination<CommonLookupDto>>('GET', this.apiName + '/common-lookup', {params:<any>req});
  }

  createCommonLookup( req: CommonLookupDto ) : Observable<CommonLookupDto> {
    return this.http.request<CommonLookupDto>('POST', this.apiName + '/common-lookup', { body:req});
  }

  updateCommonLookupById(body: CommonLookupDto, id: string): Observable<CommonLookupDto> {
    return this.http.request<CommonLookupDto>('PUT',  this.apiName + `/common-lookup/${id}`, { body });
  }
  deleteCommonLookupById(id: string): Observable<void> {
    return this.http.request<void>('DELETE', this.apiName + `/common-lookup/${id}`);
  }
  getCommonLookupById(id: string): Observable<CommonLookupDto> {
    return this.http.request<CommonLookupDto>('GET', this.apiName + `/common-lookup/${id}`);
  }
}
