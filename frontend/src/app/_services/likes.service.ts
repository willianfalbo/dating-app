import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { DATINGAPP_API_URL } from '../app.config';
import { User } from '../_models/user';
import { Paginated } from '../_models/pagination';

import { Helper } from './helper';

@Injectable({
  providedIn: 'root'
})
export class LikesService {

  constructor(private http: HttpClient) { }

  getLikes(page?: number, limit?: number, filterSender?: boolean): Observable<Paginated<User>> {
    let params = new HttpParams();

    if (page) {
      params = params.append('page', page.toString());
    }
    if (limit) {
      params = params.append('limit', limit.toString());
    }

    params = params.append('filterSender', filterSender.toString());

    return this.http.get<Paginated<User>>(`${DATINGAPP_API_URL}/likes`, { params })
      .pipe(
        map(response => {
          // set default photo in case of null
          return {
            ...response,
            items: response.items.map(u => {
              return {
                ...u,
                photoUrl: Helper.checkEmptyUserPhoto(u.photoUrl, u.gender),
              };
            }),
          };
        })
      );
  }

  sendLike(receiverId: number) {
    return this.http.post(`${DATINGAPP_API_URL}/likes`, { receiverId });
  }

}
