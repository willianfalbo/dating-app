import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { map } from 'rxjs/operators';
import { DATINGAPP_API } from '../app.api';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) { }

  login(model: any) {
    return this.http.post(`${DATINGAPP_API}/auth/login`, model).pipe(
      map((response: any) => {
        if (response) {
          localStorage.setItem('token', response.token);
        }
      })
    );
  }

  register(model: any) {
    return this.http.post(`${DATINGAPP_API}/auth/register`, model);
  }

}
