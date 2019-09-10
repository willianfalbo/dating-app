import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

import { Observable } from 'rxjs';
import { tap, map } from 'rxjs/operators';

import { DATINGAPP_API_URL } from '../app.settings';
import { User } from '../_models/user';
import { PaginatedResult } from '../_models/pagination';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }

  getUsers(pageNumber?: number, pageSize?: number, userParams?: any): Observable<PaginatedResult<User[]>> {
    const paginatedResult: PaginatedResult<User[]> = new PaginatedResult<User[]>();

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
          paginatedResult.result.forEach(p => {
            p = this.checkUserPhoto(p);
          });

          return paginatedResult;
        })
      );
  }

  getUser(id: number): Observable<User> {
    return this.http.get<User>(`${DATINGAPP_API_URL}/users/${id}`)
    .pipe(
      tap(p => {
        p = this.checkUserPhoto(p); // set default photo in case of null
      })
    );
  }

  updateUser(id: number, user: User) {
    return this.http.put(`${DATINGAPP_API_URL}/users/${id}`, user);
  }

  setMainPhoto(userId: number, userPhotoId: number) {
    return this.http.put(`${DATINGAPP_API_URL}/users/${userId}/photos/${userPhotoId}/setMain`, {});
  }

  deleteUserPhoto(userId: number, userPhotoId: number) {
    return this.http.delete(`${DATINGAPP_API_URL}/users/${userId}/photos/${userPhotoId}`);
  }

  checkUserGender(gender: string): string {
    gender = gender.toLowerCase();
    if (gender === 'male' || gender === 'female') {
      return gender;
    } else {
      return 'unknown';
    }
  }

  checkUserPhoto(user: User) {
    if (!user.photoUrl || user.photoUrl.trim() === '') {
      user.photoUrl = `../../assets/gender/${this.checkUserGender(user.gender)}.png`;
    }
    return user;
  }

}
