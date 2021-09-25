import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

import { Observable } from 'rxjs';
import { tap, map } from 'rxjs/operators';

import { DATINGAPP_API_URL } from '../app.config';
import { User } from '../_models/user';
import { Paginated } from '../_models/pagination';

import { Helper } from './helper';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  constructor(private http: HttpClient) { }

  getUsers(page?: number, limit?: number, userParams?: any): Observable<Paginated<User>> {
    let params = new HttpParams();

    if (page) {
      params = params.append('page', page.toString());
    }
    if (limit) {
      params = params.append('limit', limit.toString());
    }
    if (userParams) {
      params = params.append('minAge', userParams.minAge);
      params = params.append('maxAge', userParams.maxAge);
      params = params.append('gender', userParams.gender);
      params = params.append('orderBy', userParams.orderBy);
    }

    return this.http.get<Paginated<User>>(`${DATINGAPP_API_URL}/users`, { params })
      .pipe(
        map(response => {
          return {
            ...response,
            // set default photo in case of null
            items: response.items.map(u => {
              return {
                ...u,
                photoUrl: Helper.checkEmptyUserPhoto(u.photoUrl, u.gender),
              }
            })
          };
        })
      );
  }

  getUser(id: number): Observable<User> {
    return this.http.get<User>(`${DATINGAPP_API_URL}/users/${id}`)
      .pipe(
        map(user => {
          return {
            ...user,
            photoUrl: Helper.checkEmptyUserPhoto(user.photoUrl, user.gender),
          };
        })
      );
  }

  updateUser(user: User) {
    return this.http.put(`${DATINGAPP_API_URL}/users`, user);
  }

}
