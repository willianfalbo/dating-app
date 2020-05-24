import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { User } from '../_models/user';
import { UserService } from '../_services/user.service';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class MembersResolver implements Resolve<User[]> {

    pageNumber = 1;
    pageSize = 5;
    user: User;
    userParams: any = {};

    constructor(private userService: UserService, private authService: AuthService,
        private router: Router, private alertify: AlertifyService) { }

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<User[]> {

        this.setDefaultFilters();

        return this.userService.getUsers(this.pageNumber, this.pageSize, this.userParams).pipe(
            catchError(error => {
                this.alertify.error('Problem retrieving data');
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
