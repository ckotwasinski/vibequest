import { Injectable } from '@angular/core';
import { Observable, ObservedValueOf } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { RolesAddUpdateDto, RolesDto } from '../models';
import { UsersDto } from '../models/users-dto';
import { UsersAddUpdateDto } from '../models/users-add-update-dto';
import { Pagination, PaginationRequstDto } from 'src/app/shared/models/pagination';
import { EventsDto } from '../models/events-dto';
import { EventCategoryDto } from '../models/event-category-dto';
import { EventCategoryAddUpdateDto } from '../models/event-category-add-update-dto';

@Injectable({
  providedIn: 'root'
})
export class IdentityManagementService {
  apiName = environment.oAuthConfig.issuer;

  constructor(private http: HttpClient) { }

  getRoleList(request: PaginationRequstDto): Observable<Pagination<RolesDto>> {
    return this.http.request<Pagination<RolesDto>>('GET', this.apiName + '/role', { params: <any>request });
  }
  createRoleByInput(body: RolesAddUpdateDto): Observable<void> {
    return this.http.request<void>('POST', this.apiName + '/role', { body });
  }
  updateRoleByIdAndInput(body: RolesAddUpdateDto, id: string): Observable<void> {
    return this.http.request<void>('PUT', this.apiName + `/role/${id}`, { body });
  }
  deleteRoleById(id: string): Observable<void> {
    return this.http.request<void>('DELETE', this.apiName + `/role/${id}`);
  }
  getRoleById(id: string): Observable<RolesAddUpdateDto> {
    return this.http.request<RolesAddUpdateDto>('GET', this.apiName + `/role/${id}`);
  }
  getUsersList(request: PaginationRequstDto): Observable<Pagination<UsersDto>> {
    return this.http.request<Pagination<UsersDto>>('GET', this.apiName + '/user', { params: <any>request });
  }
  getEventsList(request: PaginationRequstDto): Observable<Pagination<EventsDto>> {
    return this.http.request<Pagination<EventsDto>>('GET', this.apiName + '/event', { params: <any>request });
  }
  deleteEventById(id: string): Observable<void> {
    return this.http.request<void>('DELETE', this.apiName + `/event/${id}`);
  }
  getEventCategoryList(request: PaginationRequstDto): Observable<Pagination<EventCategoryDto>> {
    return this.http.request<Pagination<EventCategoryDto>>('GET', this.apiName + '/event-category', { params: <any>request });
  }
  createEventCategoryByInput(body: EventCategoryAddUpdateDto): Observable<EventCategoryDto> {
    return this.http.request<EventCategoryDto>('POST', this.apiName + '/event-category',  { body });
  }
  updateEventCategoryByIdAndInput(body: EventCategoryAddUpdateDto, id: string): Observable<EventCategoryDto> {
    return this.http.request<EventCategoryDto>('PUT', this.apiName + `/event-category/${id}`, { body });
  }
  getEventCategoryById(id: string): Observable<EventCategoryAddUpdateDto> {
    return this.http.request<EventCategoryAddUpdateDto>('GET', this.apiName + `/event-category/${id}`);
  }
  deleteEventCategoryById(id: string): Observable<void> {
    return this.http.request<void>('DELETE', this.apiName + `/event-category/${id}`);
  }
  createUserByInput(body: UsersAddUpdateDto): Observable<UsersDto> {
    return this.http.request<UsersDto>('POST', this.apiName + '/user', { body });
  }
  updateUserByIdAndInput(body: UsersAddUpdateDto, id: string): Observable<UsersDto> {
    return this.http.request<UsersDto>('PUT', this.apiName + `/user/${id}`, { body });
  }
  deleteUserById(id: string): Observable<void> {
    return this.http.request<void>('DELETE', this.apiName + `/user/${id}`);
  }
  getUserById(id: string): Observable<UsersAddUpdateDto> {
    return this.http.request<UsersAddUpdateDto>('GET', this.apiName + `/user/${id}`);
  }
  uploadProfilePhoto(body: FormData, id: string): Observable<void> {
    return this.http.request<void>('POST', this.apiName + `/Account/${id}/upload-profile-image`, { body: body });
  }
  uploadEventCategoryPhoto(body: FormData, id: string): Observable<void> {
    return this.http.request<void>('POST', this.apiName + `/event-category/${id}/upload-event-category-image`, { body: body });
  }
  getRoleListDropdown(): Observable<RolesDto[]> {
    return this.http.request<RolesDto[]>('GET', this.apiName + '/role/dropdown-list');
  }
}
