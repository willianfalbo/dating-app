import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/_models/user';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from 'ngx-gallery';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {

  user: User;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];

  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.loadMemberDetails();
    this.loadUserPhotos();
  }

  private loadMemberDetails() {
    // get data before activating the route. It can be used to avoid using safe navigators "?" in html page
    this.route.data.subscribe(data => {
      this.user = data['userResolver'];
    });
  }

  // this function was built based on
  // https://www.npmjs.com/package/ngx-gallery
  private loadUserPhotos() {
    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false,
      }
    ];
    this.galleryImages = this.getImages();
  }

  getImages() {
    const imageUrls = [];
    this.user.photos.forEach(element => {
      imageUrls.push({
        small: element.url,
        medium: element.url,
        big: element.url,
        description: element.description,
      });
    });
    return imageUrls;
  }

}
