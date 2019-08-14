import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { JwtHelperService } from '@auth0/angular-jwt';

import { map } from 'rxjs/operators';
import { DATINGAPP_API_URL, TOKEN_NAME } from '../app.settings';
import { DataToken } from './decoded-token.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  jwtHelper = new JwtHelperService();

  constructor(private http: HttpClient) { }

  login(model: any) {
    return this.http.post(`${DATINGAPP_API_URL}/auth/login`, model).pipe(
      map((response: any) => {
        if (response) {
          localStorage.setItem(TOKEN_NAME, response.token);
        }
      }));
  }

  logout() {
    localStorage.removeItem(TOKEN_NAME);
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

}
