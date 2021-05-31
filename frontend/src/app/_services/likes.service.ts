import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { DATINGAPP_API_URL } from '../app.config';
import { User } from '../_models/user';
import { PaginatedResult } from '../_models/pagination';

import { Helper } from './helper';

@Injectable({
  providedIn: 'root'
})
export class LikesService {

  constructor(private http: HttpClient) { }

  getLikes(pageNumber?: number, pageSize?: number, filterSender?: boolean): Observable<PaginatedResult<User[]>> {
    const paginatedResult = new PaginatedResult<User[]>();

    let params = new HttpParams();

    if (pageNumber) {
      params = params.append('pageNumber', pageNumber.toString());
    }
    if (pageSize) {
      params = params.append('pageSize', pageSize.toString());
    }

    params = params.append('filterSender', filterSender.toString());

    return this.http.get<User[]>(`${DATINGAPP_API_URL}/likes`, { observe: 'response', params })
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

  sendLike(receiverId: number) {
    return this.http.post(`${DATINGAPP_API_URL}/likes`, { receiverId });
  }

}
