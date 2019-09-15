import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { JwtHelperService } from '@auth0/angular-jwt';

import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';

import { DATINGAPP_API_URL, TOKEN_NAME, USER_OBJECT_NAME } from '../app.settings';
import { DecodedToken } from '../_models/decoded-token.model';
import { User } from '../_models/user';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  jwtHelper = new JwtHelperService();
  decodedToken: DecodedToken;

  private _currentUser: User;
  get currentUser(): User {
    return this._currentUser;
  }

  // creates an observable for users
  // it will be used when users set a different Main Photo or update their display name
  // so all components that subscribes to this will be notified
  currentUserBehavior = new BehaviorSubject<User>(new User());
  currentUserObservable = this.currentUserBehavior.asObservable();

  constructor(private http: HttpClient, private userService: UserService) { }

  login(model: any) {
    return this.http.post(`${DATINGAPP_API_URL}/auth/login`, model).pipe(
      map((response: any) => {
        if (response) {
          // store token in local storage
          const token = response.token;
          localStorage.setItem(TOKEN_NAME, token);
          this.decodedToken = this.getDecodedToken(token);
          // update member information
          this.updateMember(response.user);
        }
      }));
  }

  logout() {
    localStorage.removeItem(TOKEN_NAME);
    localStorage.removeItem(USER_OBJECT_NAME);
    this._currentUser = null;
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

  getDecodedToken(token: string): DecodedToken {
    if (token) {
      const data = this.jwtHelper.decodeToken(token);
      if (!data.nameid) {
        throw new Error('Expected UserId property');
      }
      if (!data.unique_name) {
        throw new Error('Expected UserName property');
      }
      return new DecodedToken(data.nameid, data.unique_name);
    }
  }

  updateMember(user: User) {
    user = this.userService.checkUserPhoto(user);
    // change user's information in local storage
    this._currentUser = user;
    localStorage.setItem(USER_OBJECT_NAME, JSON.stringify(this._currentUser));
    // emit event for all subscribers
    this.currentUserBehavior.next(user);
  }

  updateMemberPhoto(photoUrl: string) {
    const userToChange = this._currentUser;
    userToChange.photoUrl = photoUrl;
    this.updateMember(userToChange);
  }

}
