import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { User } from '../_models/user';
import { Pagination, PaginatedResult } from '../_models/pagination';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-members',
  templateUrl: './members.component.html',
  styleUrls: ['./members.component.css']
})
export class MembersComponent implements OnInit {

  users: User[];
  user: User;
  genders: any[] = [
    { value: 'male', display: 'Males' },
    { value: 'female', display: 'Females' },
    { value: 'unknown', display: 'Others' }
  ];
  userParams: any = {};
  pagination: Pagination;

  constructor(private route: ActivatedRoute, private userService: UserService,
    private alertify: AlertifyService, private authService: AuthService) { }

  ngOnInit() {
    // get data before activating the route. It can be used to avoid using safe navigators "?" in html page
    this.route.data.subscribe(data => {
      this.users = data['usersResolver'].result;
      this.pagination = data['usersResolver'].pagination;
    });
    this.user = this.authService.currentUser;
    this.setDefaultFilters();
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadUsers();
  }

  loadUsers() {
    this.userService
      .getUsers(this.pagination.currentPage, this.pagination.itemsPerPage, this.userParams)
      .subscribe(
        (res: PaginatedResult<User[]>) => {
          this.users = res.result;
          this.pagination = res.pagination;
        }, error => {
          this.alertify.error(error);
        }
      );
  }

  private setDefaultFilters() {
    if (this.user.gender === 'unknown') {
      this.userParams.gender = 'unknown';
    } else {
      this.userParams.gender = this.user.gender === 'female' ? 'male' : 'female';
    }
    this.userParams.minAge = 18;
    this.userParams.maxAge = 99;
    this.userParams.orderBy = 'lastActive';
  }

  resetFilters() {
    this.setDefaultFilters();
    this.loadUsers();
  }

}
