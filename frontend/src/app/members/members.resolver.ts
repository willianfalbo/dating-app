import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { User } from '../_models/user';
import { UsersService } from '../_services/users.service';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Paginated } from '../_models/pagination';

@Injectable()
export class MembersResolver implements Resolve<Paginated<User>> {

  page = 1;
  limit = 5;
  user: User;
  userParams: any = {};

  constructor(
    private userService: UsersService,
    private authService: AuthService,
    private router: Router,
    private alertify: AlertifyService
  ) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<Paginated<User>> {

    this.setDefaultFilters();

    return this.userService.getUsers(this.page, this.limit, this.userParams).pipe(
      catchError(error => {
        this.alertify.error('Problem retrieving data.');
        this.router.navigate(['/home']);
        return of(null);
      })
    );
  }

  private setDefaultFilters() {
    this.user = this.authService.currentUser;

    if (this.user.gender === 'unknown') {
      this.userParams.gender = 'unknown';
    } else {
      this.userParams.gender = this.user.gender === 'female' ? 'male' : 'female';
    }
    this.userParams.minAge = 18;
    this.userParams.maxAge = 99;
    this.userParams.orderBy = 'lastActive';
  }
}
