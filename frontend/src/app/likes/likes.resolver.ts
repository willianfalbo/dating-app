import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { User } from '../_models/user';
import { AlertifyService } from '../_services/alertify.service';
import { LikesService } from '../_services/likes.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Paginated } from '../_models/pagination';

@Injectable()
export class LikesResolver implements Resolve<Paginated<User>> {

  page = 1;
  limit = 5;
  likerKind = 'sender';

  constructor(
    private likesService: LikesService,
    private router: Router,
    private alertify: AlertifyService
  ) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<Paginated<User>> {
    return this.likesService.getLikes(this.page, this.limit, this.likerKind === 'sender')
      .pipe(
        catchError(error => {
          this.alertify.error('Problem retrieving data.');
          this.router.navigate(['/home']);
          return of(null);
        })
      );
  }
}
