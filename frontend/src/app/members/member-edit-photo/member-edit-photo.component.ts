import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { UserPhoto } from 'src/app/_models/userPhoto';
import { FileUploader } from 'ng2-file-upload';
import { DATINGAPP_API_URL } from 'src/app/app.settings';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-member-edit-photo',
  templateUrl: './member-edit-photo.component.html',
  styleUrls: ['./member-edit-photo.component.css']
})
export class MemberEditPhotoComponent implements OnInit {

  @Input() photos: UserPhoto[];

  uploader: FileUploader;
  hasBaseDropZoneOver: false;
  currentMainPhoto: UserPhoto;

  constructor(private authService: AuthService, private userService: UserService,
    private alertify: AlertifyService) { }

  ngOnInit() {
    this.initializeUploader();
  }

  public fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: `${DATINGAPP_API_URL}/users/${this.authService.decodedToken.userId}/photos`,
      authToken: `Bearer ${this.authService.getToken()}`,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024 // 10MB
    });

    // to fix the following error:
    // Access to XMLHttpRequest at '...' from origin '...' has been blocked by CORS policy: Response to preflight request doesn't pass access control check: The value of the 'Access-Control-Allow-Origin' header in the response must not be the wildcard '*' when the request's credentials mode is 'include'. The credentials mode of requests initiated by the XMLHttpRequest is controlled by the withCredentials attribute.
    this.uploader.onAfterAddingFile = (file) => { file.withCredentials = false; };

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const resp: UserPhoto = JSON.parse(response);
        const userPhoto: UserPhoto = {
          id: resp.id,
          url: resp.url,
          dateAdded: resp.dateAdded,
          description: resp.description,
          isMain: resp.isMain,
          isApproved: resp.isApproved,
        };
        this.photos.push(userPhoto);
        if (userPhoto.isMain) {
          this.authService.updateMemberPhoto(userPhoto.url);
        }
      }
    };
  }

  setMainPhoto(userPhoto: UserPhoto) {
    this.userService.setMainPhoto(+this.authService.decodedToken.userId, userPhoto.id)
      .subscribe(next => {
        this.currentMainPhoto = this.photos.filter(p => p.isMain === true)[0];
        this.currentMainPhoto.isMain = false;
        userPhoto.isMain = true;
        this.authService.updateMemberPhoto(userPhoto.url);
        this.alertify.success('Successfully set to main');
      }, error => {
        this.alertify.error(error);
      });
  }

  deleteUserPhoto(userPhotoId: number) {
    this.alertify.confirm('Are you sure you want to delete this photo?', () => {
      this.userService.deleteUserPhoto(+this.authService.decodedToken.userId, userPhotoId)
        .subscribe(result => {
          // remove deleted photo from the array
          this.photos = this.photos.filter(p => p.id !== userPhotoId);
          this.alertify.success('Photo has been deleted');
        }, error => {
          this.alertify.error(error);
        });
    });
  }

}
