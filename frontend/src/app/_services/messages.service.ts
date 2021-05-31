import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

import { Observable } from 'rxjs';
import { tap, map } from 'rxjs/operators';

import { DATINGAPP_API_URL } from '../app.settings';
import { PaginatedResult } from '../_models/pagination';
import { Message } from '../_models/message';

import { Helper } from './helper';

@Injectable({
  providedIn: 'root'
})
export class MessagesService {

  constructor(private http: HttpClient) { }

  getMessages(pageNumber?: number, pageSize?: number, messageContainer?: string): Observable<PaginatedResult<Message[]>> {
    const paginatedResult = new PaginatedResult<Message[]>();

    let params = new HttpParams();

    params = params.append('container', messageContainer);

    if (pageNumber) {
      params = params.append('pageNumber', pageNumber.toString());
    }
    if (pageSize) {
      params = params.append('pageSize', pageSize.toString());
    }

    return this.http.get<Message[]>(`${DATINGAPP_API_URL}/messages`, { observe: 'response', params })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          // set default photo in case of null
          paginatedResult.result.forEach(m => {
            m.senderPhotoUrl = Helper.checkEmptyUserPhoto(m.senderPhotoUrl, m.senderGender);
            m.recipientPhotoUrl = Helper.checkEmptyUserPhoto(m.recipientPhotoUrl, m.recipientGender);
          });
          return paginatedResult;
        })
      );
  }

  getMessagesThread(recipientId: number) {
    return this.http.get<Message[]>(`${DATINGAPP_API_URL}/messages/thread/${recipientId}`)
      .pipe(
        tap(messages => {
          messages.forEach(m => {
            m.senderPhotoUrl = Helper.checkEmptyUserPhoto(m.senderPhotoUrl, m.senderGender);
            m.recipientPhotoUrl = Helper.checkEmptyUserPhoto(m.recipientPhotoUrl, m.recipientGender);
          });
        })
      );
  }

  sendMessage(message: Message) {
    return this.http.post(`${DATINGAPP_API_URL}/messages`, message);
  }

  deleteMessage(messageId: number) {
    return this.http.delete(`${DATINGAPP_API_URL}/messages/${messageId}/delete`);
  }

  markSenderMessagesAsRead(recipientId: number) {
    return this.http.post(`${DATINGAPP_API_URL}/messages/thread/${recipientId}/mark-as-read`, {});
  }

}
