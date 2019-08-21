import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { JwtHelperService } from '@auth0/angular-jwt';

import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';

import { DATINGAPP_API_URL, TOKEN_NAME, USER_OBJECT_NAME } from '../app.settings';
import { DataToken } from './decoded-token.model';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  jwtHelper = new JwtHelperService();
  userPhotoUrl = new BehaviorSubject<string>('');
  currentUserPhotoUrl = this.userPhotoUrl.asObservable();

  constructor(private http: HttpClient) { }

  login(model: any) {
    return this.http.post(`${DATINGAPP_API_URL}/auth/login`, model).pipe(
      map((response: any) => {
        if (response) {
          localStorage.setItem(TOKEN_NAME, response.token);
          localStorage.setItem(USER_OBJECT_NAME, JSON.stringify(response.user));
          this.changeMemberPhoto(this.getUser().photoUrl);
        }
      }));
  }

  logout() {
    localStorage.removeItem(TOKEN_NAME);
    localStorage.removeItem(USER_OBJECT_NAME);
  }

  register(model: any) {
    return this.http.post(`${DATINGAPP_API_URL}/auth/register`, model);
  }

  isLoggedIn() {
    const token = this.getToken();
    return !this.jwtHelper.isTokenExpired(token);
  }

  getToken() {
    return localStorage.getItem(TOKEN_NAME);
  }

  getUser(): User {
    return JSON.parse(localStorage.getItem(USER_OBJECT_NAME));
  }

  getDecodedToken(): DataToken {
    const token = this.getToken();
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

  changeMemberPhoto(userPhotoUrl: string) {
    if (!userPhotoUrl || userPhotoUrl.trim() === '') {
      userPhotoUrl = `../../assets/gender/${this.getUserGender(this.getUser().gender)}.png`;
    }
    // change user's photo from local storage
    const currentUser = this.getUser();
    currentUser.photoUrl = userPhotoUrl;
    localStorage.setItem(USER_OBJECT_NAME, JSON.stringify(currentUser));
    // emit event for all subscribers
    this.userPhotoUrl.next(userPhotoUrl);
  }

  getUserGender(gender: string): string {
    let userGender = 'unknown';
    if (gender) {
      userGender = gender.toLowerCase();
    }
    return userGender;
  }

}
