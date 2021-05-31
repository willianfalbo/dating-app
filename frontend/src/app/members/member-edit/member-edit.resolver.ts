import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { User } from '../../_models/user';
import { UsersService } from '../../_services/users.service';
import { AlertifyService } from '../../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from 'src/app/_services/auth.service';

@Injectable()
export class MemberEditResolver implements Resolve<User> {

  constructor(
    private userService: UsersService,
    private authService: AuthService,
    private router: Router,
    private alertify: AlertifyService
  ) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<User> {
    return this.userService.getUser(+this.authService.decodedToken.userId).pipe(
      catchError(error => {
        this.alertify.error('Problem retrieving your data.');
        this.router.navigate(['/members']);
        return of(null);
      })
    );
  }
}
