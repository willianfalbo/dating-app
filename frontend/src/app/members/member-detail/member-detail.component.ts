import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from 'ngx-gallery';
import { TabsetComponent } from 'ngx-bootstrap';

import { User } from 'src/app/_models/user';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { LikesService } from 'src/app/_services/likes.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {

  @ViewChild('memberTabs') memberTabs: TabsetComponent;
  user: User;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];

  constructor(
    private route: ActivatedRoute,
    private likesService: LikesService,
    private alertify: AlertifyService
  ) { }

  ngOnInit() {
    // get data before activating the route. It can be used to avoid using safe navigators "?" in html page
    this.route.data.subscribe(data => {
      this.user = data['userResolver'];
    });

    this.route.queryParams.subscribe(params => {
      const tabId = params['tab'];
      if (tabId) {
        this.selectTab(tabId);
      }
    });

    this.loadUserPhotos();
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

  selectTab(tabId: number) {
    // decrease one in order to start tabs by 1,2,3...
    tabId = (tabId - 1);
    // check tab index in order to mitigate errors
    if (tabId < 0 || tabId > 3) {
      tabId = 0;
    }
    this.memberTabs.tabs[tabId].active = true;
  }

  sendLike(recipientId: number) {
    this.likesService.sendLike(recipientId).subscribe(data => {
      this.alertify.success(`You have liked ${this.user.knownAs}.`);
    }, error => {
      if (error.status === 409)
        this.alertify.warning(error.error);
      else
        this.alertify.error(error.error);
    });
  }

}
