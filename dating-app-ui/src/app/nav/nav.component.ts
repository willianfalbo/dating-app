import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { User } from '../_models/user';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {};
  userName: string;
  photoUrl: string;

  constructor(private authService: AuthService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {

    // subscribe to user's photo changes
    this.authService.currentUserPhotoUrl.subscribe(url => {
      this.photoUrl = url;
    });

    // get user name from token
    this.getUserName();

    // change user photo when loading nav component
    this.authService.changeMemberPhoto(this.authService.getUser().photoUrl);
  }

  private getUserName() {
    const dataToken = this.authService.getDecodedToken();
    if (dataToken) {
      this.userName = dataToken.userName;
    }
  }

  login() {
    this.authService.login(this.model).subscribe(next => {
      this.getUserName();
      this.alertify.success('Logged in successfully');
    }, error => {
      this.alertify.error(error);
    }, () => {
      this.router.navigate(['/members']);
    });
  }

  isLoggedIn() {
    return this.authService.isLoggedIn();
  }

  logout() {
    this.authService.logout();
    this.alertify.message('logged out');
    this.router.navigate(['/home']);
  }
}
