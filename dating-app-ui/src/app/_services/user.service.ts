import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

import { Observable } from 'rxjs';
import { tap, map } from 'rxjs/operators';

import { DATINGAPP_API_URL } from '../app.settings';
import { User } from '../_models/user';
import { PaginatedResult } from '../_models/pagination';
import { Message } from '../_models/message';
import { AlertifyService } from './alertify.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient, private alertify: AlertifyService) { }

  getUsers(pageNumber?: number, pageSize?: number, userParams?: any, likesParam?: string): Observable<PaginatedResult<User[]>> {
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
    if (likesParam) {
      if (likesParam.toLowerCase() === 'likers') {
        params = params.append('likers', 'true');
      }
      if (likesParam.toLowerCase() === 'likees') {
        params = params.append('likees', 'true');
      }
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
            u = this.checkUserPhoto(u);
          });
          return paginatedResult;
        })
      );
  }

  getUser(id: number): Observable<User> {
    return this.http.get<User>(`${DATINGAPP_API_URL}/users/${id}`)
      .pipe(
        tap(u => {
          u = this.checkUserPhoto(u); // set default photo in case of null
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
    if (gender) {
      gender = gender.toLowerCase().trim();
      if (gender === 'male' || gender === 'female' || gender === 'unknown') {
        return gender;
      } else {
        return 'unknown';
      }
    } else {
      return 'unknown';
    }
  }

  checkUserPhoto(user: User): User {
    user.photoUrl = this.checkEmptyUserPhoto(user.photoUrl, user.gender);
    return user;
  }

  checkEmptyUserPhoto(photoUrl: string, gender: string): string {
    if (!photoUrl || photoUrl.trim() === '') {
      photoUrl = `../../assets/gender/${this.checkUserGender(gender)}.png`;
    }
    return photoUrl;
  }

  sendLike(userId: number, recipientId: number) {
    return this.http.post(`${DATINGAPP_API_URL}/users/${userId}/like/${recipientId}`, {});
  }

  getMessages(id: number, pageNumber?: number, pageSize?: number, messageFontainer?: string): Observable<PaginatedResult<Message[]>> {
    const paginatedResult = new PaginatedResult<Message[]>();

    let params = new HttpParams();

    params = params.append('container', messageFontainer);

    if (pageNumber) {
      params = params.append('pageNumber', pageNumber.toString());
    }
    if (pageSize) {
      params = params.append('pageSize', pageSize.toString());
    }

    return this.http.get<Message[]>(`${DATINGAPP_API_URL}/users/${id}/messages`, { observe: 'response', params })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          // set default photo in case of null
          paginatedResult.result.forEach(m => {
            m.senderPhotoUrl = this.checkEmptyUserPhoto(m.senderPhotoUrl, m.senderGender);
            m.recipientPhotoUrl = this.checkEmptyUserPhoto(m.recipientPhotoUrl, m.recipientGender);
          });
          return paginatedResult;
        })
      );
  }

  getMessagesThread(userId: number, recipientId: number) {
    return this.http.get<Message[]>(`${DATINGAPP_API_URL}/users/${userId}/messages/thread/${recipientId}`)
    .pipe(
      tap(messages => {
        messages.forEach(m => {
          m.senderPhotoUrl = this.checkEmptyUserPhoto(m.senderPhotoUrl, m.senderGender);
          m.recipientPhotoUrl = this.checkEmptyUserPhoto(m.recipientPhotoUrl, m.recipientGender);
        });
      })
    );
  }

  sendMessage(userId: number, message: Message) {
    return this.http.post(`${DATINGAPP_API_URL}/users/${userId}/messages`, message);
  }

  deleteMessage(userId: number, id: number) {
    return this.http.delete(`${DATINGAPP_API_URL}/users/${userId}/messages/${id}/delete`);
  }

  markSenderMessagesAsRead(userId: number, recipientId: number) {
    return this.http.post(`${DATINGAPP_API_URL}/users/${userId}/messages/thread/${recipientId}/mark-as-read`, {});
  }

}
