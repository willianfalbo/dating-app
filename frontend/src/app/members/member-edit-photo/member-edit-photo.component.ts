import { Component, OnInit, Input } from '@angular/core';
import { Photo } from 'src/app/_models/photo';
import { FileUploader } from 'ng2-file-upload';
import { DATINGAPP_API_URL } from 'src/app/app.config';

import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { PhotosService } from 'src/app/_services/photos.service';

@Component({
  selector: 'app-member-edit-photo',
  templateUrl: './member-edit-photo.component.html',
  styleUrls: ['./member-edit-photo.component.css']
})
export class MemberEditPhotoComponent implements OnInit {

  @Input() photos: Photo[];

  uploader: FileUploader;
  hasBaseDropZoneOver: false;
  currentMainPhoto: Photo;

  constructor(
    private authService: AuthService,
    private photosService: PhotosService,
    private alertify: AlertifyService
  ) { }

  ngOnInit() {
    this.initializeUploader();
  }

  public fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: `${DATINGAPP_API_URL}/photos`,
      authToken: `Bearer ${this.authService.getToken()}`,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024 // 10MB
    });

    // to fix cors erros.
    this.uploader.onAfterAddingFile = (file) => { file.withCredentials = false; };

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const resp: Photo = JSON.parse(response);
        const photo: Photo = {
          id: resp.id,
          url: resp.url,
          dateAdded: resp.dateAdded,
          description: resp.description,
          isMain: resp.isMain,
          isApproved: resp.isApproved,
        };
        this.photos.push(photo);
        if (photo.isMain) {
          this.authService.updateMemberPhoto(photo.url);
        }
      }
    };
  }

  setMainPhoto(userPhoto: Photo) {
    this.photosService.setMainPhoto(userPhoto.id)
      .subscribe(next => {
        this.currentMainPhoto = this.photos.filter(p => p.isMain === true)[0];
        this.currentMainPhoto.isMain = false;
        userPhoto.isMain = true;
        this.authService.updateMemberPhoto(userPhoto.url);
        this.alertify.success('Successfully set to main.');
      }, error => {
        this.alertify.error(error.error);
      });
  }

  deleteUserPhoto(userPhotoId: number) {
    this.alertify.confirm('Are you sure you want to delete this photo?', () => {
      this.photosService.deleteUserPhoto(userPhotoId)
        .subscribe(result => {
          // remove deleted photo from the array
          this.photos = this.photos.filter(p => p.id !== userPhotoId);
          this.alertify.success('Photo has been deleted.');
        }, error => {
          this.alertify.error(error.error);
        });
    });
  }

}
