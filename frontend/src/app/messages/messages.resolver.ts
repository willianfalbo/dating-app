import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { AlertifyService } from '../_services/alertify.service';
import { MessagesService } from '../_services/messages.service';

import { Message } from '../_models/message';
import { Paginated } from '../_models/pagination';

@Injectable()
export class MessagesResolver implements Resolve<Paginated<Message>> {

  page = 1;
  limit = 5;
  messageContainer = 'Unread';

  constructor(
    private messagesService: MessagesService,
    private router: Router,
    private alertify: AlertifyService
  ) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<Paginated<Message>> {
    return this.messagesService.getMessages(this.page, this.limit, this.messageContainer)
      .pipe(
        catchError(error => {
          this.alertify.error('Problem retrieving messages.');
          this.router.navigate(['/home']);
          return of(null);
        })
      );
  }
}
