import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DATINGAPP_API_URL } from '../app.setting';
import { User } from '../_models/user';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(`${DATINGAPP_API_URL}/users`);
  }

  getUser(id: number): Observable<User> {
    return this.http.get<User>(`${DATINGAPP_API_URL}/users/${id}`);
  }

}
