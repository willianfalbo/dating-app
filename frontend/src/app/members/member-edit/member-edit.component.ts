import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { User } from 'src/app/_models/user';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { NgForm } from '@angular/forms';
import { UsersService } from 'src/app/_services/users.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {

  @ViewChild('editForm') editForm: NgForm;
  user: User;
  photoUrl: string;

  // prevent unsaved changes when user clicks on closing the window
  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(
    private route: ActivatedRoute,
    private alertify: AlertifyService,
    private userService: UsersService,
    private authService: AuthService
  ) { }

  ngOnInit() {
    this.loadMemberEdit();

    // subscribe to user's changes
    this.authService.currentUserObservable.subscribe(user => {
      if (user) {
        this.photoUrl = user.photoUrl;
      }
    });
  }

  private loadMemberEdit() {
    // get data before activating the route. It can be used to avoid using safe navigators "?" in html page
    this.route.data.subscribe(data => {
      this.user = data['userEditResolver'];
    });
  }

  updateUser() {
    this.userService.updateUser(this.user)
      .subscribe(next => {
        this.alertify.success('Profile updated successfully.');
        this.editForm.reset(this.user);
      }, error => {
        this.alertify.error(error.error);
      });
  }

}
