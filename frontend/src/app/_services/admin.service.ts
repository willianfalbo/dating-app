import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DATINGAPP_API_URL } from '../app.config';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  constructor(private http: HttpClient) { }

  getUsersWithRoles() {
    return this.http.get(`${DATINGAPP_API_URL}/admin/users-with-roles`);
  }

  updateUserRoles(user: User, roles: any) {
    return this.http.post(`${DATINGAPP_API_URL}/admin/edit-roles/${user.userName}`, roles);
  }

  getPhotosForApproval() {
    return this.http.get(`${DATINGAPP_API_URL}/admin/photos-for-moderation`);
  }

  approvePhoto(photoId) {
    return this.http.post(`${DATINGAPP_API_URL}/admin/approve-photo/${photoId}`, {});
  }

  rejectPhoto(photoId) {
    return this.http.post(`${DATINGAPP_API_URL}/admin/reject-photo/${photoId}`, {});
  }
}
