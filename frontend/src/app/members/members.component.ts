import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { UsersService } from '../_services/users.service';
import { AlertifyService } from '../_services/alertify.service';
import { User } from '../_models/user';
import { Paginated, Pagination } from '../_models/pagination';
import { AuthService } from '../_services/auth.service';
import { Genders } from '../_shared/types/genders';

type GenderOption = {
  value: Genders;
  display: string;
};

@Component({
  selector: 'app-members',
  templateUrl: './members.component.html',
  styleUrls: ['./members.component.css']
})
export class MembersComponent implements OnInit {

  users: User[];
  user: User;
  genders: GenderOption[] = [
    { value: 'male', display: 'Males' },
    { value: 'female', display: 'Females' },
    { value: 'unknown', display: 'Others' }
  ];
  userParams: any = {};
  pagination: Pagination;

  constructor(
    private route: ActivatedRoute,
    private userService: UsersService,
    private alertify: AlertifyService,
    private authService: AuthService
  ) { }

  ngOnInit() {
    // get data before activating the route. It can be used to avoid using safe navigators "?" in html page
    this.route.data.subscribe(data => {
      const { items, ...pagination } = data['usersResolver'] as Paginated<User>;
      this.users = items;
      this.pagination = pagination;
    });
    this.user = this.authService.currentUser;
    this.setDefaultFilters();
  }

  pageChanged(event: any): void {
    this.pagination.page = event.page;
    this.loadUsers();
  }

  loadUsers() {
    this.userService
      .getUsers(this.pagination.page, this.pagination.limit, this.userParams)
      .subscribe(response => {
        const { items, ...pagination } = response;
        this.users = items;
        this.pagination = pagination;
      }, error => {
        this.alertify.error(error.error);
      });
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
