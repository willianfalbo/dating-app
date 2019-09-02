import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { JwtHelperService } from '@auth0/angular-jwt';

import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';

import { DATINGAPP_API_URL, TOKEN_NAME, USER_OBJECT_NAME } from '../app.settings';
import { DataToken } from './decoded-token.model';
import { User } from '../_models/user';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  jwtHelper = new JwtHelperService();
  decodedToken: DataToken;
  currentUser: User;
  userPhotoUrl = new BehaviorSubject<string>('');
  currentUserPhotoUrl = this.userPhotoUrl.asObservable();

  constructor(private http: HttpClient, private userService: UserService) { }

  login(model: any) {
    return this.http.post(`${DATINGAPP_API_URL}/auth/login`, model).pipe(
      map((response: any) => {
        if (response) {
          // store token in local storage
          const token = response.token;
          localStorage.setItem(TOKEN_NAME, token);
          this.decodedToken = this.getDecodedToken(token);
          // store user data in local storage
          this.currentUser = this.userService.checkUserPhoto(response.user);
          localStorage.setItem(USER_OBJECT_NAME, JSON.stringify(this.currentUser));
          this.changeMemberPhoto(this.currentUser.photoUrl);
        }
      }));
  }

  logout() {
    localStorage.removeItem(TOKEN_NAME);
    localStorage.removeItem(USER_OBJECT_NAME);
    this.currentUser = null;
    this.decodedToken = null;
  }

  register(user: User) {
    return this.http.post(`${DATINGAPP_API_URL}/auth/register`, user);
  }

  isLoggedIn() {
    const token = this.getToken();
    return !this.jwtHelper.isTokenExpired(token);
  }

  getToken(): string {
    return localStorage.getItem(TOKEN_NAME);
  }

  getUser(): User {
    return JSON.parse(localStorage.getItem(USER_OBJECT_NAME));
  }

  getDecodedToken(token: string): DataToken {
    if (token) {
      const data = this.jwtHelper.decodeToken(token);
      if (!data.nameid) {
        throw new Error('Expected UserId property');
      }
      if (!data.unique_name) {
        throw new Error('Expected UserName property');
      }
      return new DataToken(data.nameid, data.unique_name);
    }
  }

  changeMemberPhoto(photoUrl: string) {
    // change user's photo in local storage
    this.currentUser.photoUrl = photoUrl;
    localStorage.setItem(USER_OBJECT_NAME, JSON.stringify(this.currentUser));
    // emit event for all subscribers
    this.userPhotoUrl.next(photoUrl);
  }

}
