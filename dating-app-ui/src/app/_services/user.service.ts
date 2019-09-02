import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DATINGAPP_API_URL } from '../app.settings';
import { User } from '../_models/user';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(`${DATINGAPP_API_URL}/users`)
      .pipe(
        tap(response =>
          response.forEach(p => {
            p = this.checkUserPhoto(p); // set default photo in case of null
          })
      ));
  }

  getUser(id: number): Observable<User> {
    return this.http.get<User>(`${DATINGAPP_API_URL}/users/${id}`)
    .pipe(
      tap(p => {
        p = this.checkUserPhoto(p); // set default photo in case of null
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
    gender = gender.toLowerCase();
    if (gender === 'male' || gender === 'female') {
      return gender;
    } else {
      return 'unknown';
    }
  }

  checkUserPhoto(user: User) {
    if (!user.photoUrl || user.photoUrl.trim() === '') {
      user.photoUrl = `../../assets/gender/${this.checkUserGender(user.gender)}.png`;
    }
    return user;
  }

}
