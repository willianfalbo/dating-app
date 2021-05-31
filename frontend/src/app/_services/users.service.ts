import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

import { Observable } from 'rxjs';
import { tap, map } from 'rxjs/operators';

import { DATINGAPP_API_URL } from '../app.config';
import { User } from '../_models/user';
import { PaginatedResult } from '../_models/pagination';

import { Helper } from './helper';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  constructor(private http: HttpClient) { }

  getUsers(pageNumber?: number, pageSize?: number, userParams?: any): Observable<PaginatedResult<User[]>> {
    const paginatedResult = new PaginatedResult<User[]>();

    let params = new HttpParams();

    if (pageNumber) {
      params = params.append('pageNumber', pageNumber.toString());
    }
    if (pageSize) {
      params = params.append('pageSize', pageSize.toString());
    }
    if (userParams) {
      params = params.append('minAge', userParams.minAge);
      params = params.append('maxAge', userParams.maxAge);
      params = params.append('gender', userParams.gender);
      params = params.append('orderBy', userParams.orderBy);
    }

    return this.http.get<User[]>(`${DATINGAPP_API_URL}/users`, { observe: 'response', params })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          // set default photo in case of null
          paginatedResult.result.forEach(u => {
            u = Helper.checkUserPhoto(u);
          });
          return paginatedResult;
        })
      );
  }

  getUser(id: number): Observable<User> {
    return this.http.get<User>(`${DATINGAPP_API_URL}/users/${id}`)
      .pipe(
        tap(u => {
          u = Helper.checkUserPhoto(u); // set default photo in case of null
        })
      );
  }

  updateUser(user: User) {
    return this.http.put(`${DATINGAPP_API_URL}/users`, user);
  }

}
