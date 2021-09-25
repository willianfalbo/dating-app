import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

import { Observable } from 'rxjs';
import { tap, map } from 'rxjs/operators';

import { DATINGAPP_API_URL } from '../app.config';
import { Paginated } from '../_models/pagination';
import { Message } from '../_models/message';

import { Helper } from './helper';

@Injectable({
  providedIn: 'root'
})
export class MessagesService {

  constructor(private http: HttpClient) { }

  getMessages(page?: number, limit?: number, messageContainer?: string): Observable<Paginated<Message>> {
    let params = new HttpParams();

    params = params.append('container', messageContainer);

    if (page) {
      params = params.append('page', page.toString());
    }
    if (limit) {
      params = params.append('limit', limit.toString());
    }

    return this.http.get<Paginated<Message>>(`${DATINGAPP_API_URL}/messages`, { params })
      .pipe(
        map(response => {
          return {
            ...response,
            items: response.items.map(m => {
              return {
                ...m,
                // set default photo in case of null
                senderPhotoUrl: Helper.checkEmptyUserPhoto(m.senderPhotoUrl, m.senderGender),
                recipientPhotoUrl: Helper.checkEmptyUserPhoto(m.recipientPhotoUrl, m.recipientGender),
              }
            })
          };
        })
      );
  }

  getMessagesThread(recipientId: number): Observable<Paginated<Message>> {
    return this.http.get<Paginated<Message>>(`${DATINGAPP_API_URL}/messages/thread/${recipientId}`)
      .pipe(
        map(response => {
          return {
            ...response,
            items: response.items.map(m => {
              return {
                ...m,
                senderPhotoUrl: Helper.checkEmptyUserPhoto(m.senderPhotoUrl, m.senderGender),
                recipientPhotoUrl: Helper.checkEmptyUserPhoto(m.recipientPhotoUrl, m.recipientGender),
              }
            }),
          };
        })
      );
  }

  sendMessage(message: Message) {
    return this.http.post<Message>(`${DATINGAPP_API_URL}/messages`, message)
      .pipe(
        map(message => {
          return {
            ...message,
            senderPhotoUrl: Helper.checkEmptyUserPhoto(message.senderPhotoUrl, message.senderGender),
            recipientPhotoUrl: Helper.checkEmptyUserPhoto(message.recipientPhotoUrl, message.recipientGender),
          };
        })
      );
  }

  deleteMessage(messageId: number) {
    return this.http.delete(`${DATINGAPP_API_URL}/messages/${messageId}/delete`);
  }

  markSenderMessagesAsRead(recipientId: number) {
    return this.http.post(`${DATINGAPP_API_URL}/messages/thread/${recipientId}/mark-as-read`, {});
  }

}
