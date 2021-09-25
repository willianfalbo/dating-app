import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Pagination, Paginated } from '../_models/pagination';
import { User } from '../_models/user';
import { AlertifyService } from '../_services/alertify.service';
import { LikesService } from '../_services/likes.service';

@Component({
  selector: 'app-likes',
  templateUrl: './likes.component.html',
  styleUrls: ['./likes.component.css']
})
export class LikesComponent implements OnInit {

  users: User[];
  pagination: Pagination;
  likerKind: any = 'sender';

  constructor(
    private likesService: LikesService,
    private route: ActivatedRoute,
    private alertify: AlertifyService
  ) { }

  ngOnInit() {
    // get data before activating the route. It can be used to avoid using safe navigators "?" in html page
    this.route.data.subscribe(data => {
      const { items, ...pagination } = data['likesResolver'] as Paginated<User>;
      this.users = items;
      this.pagination = pagination;
    });
  }

  loadUsers() {
    this.likesService
      .getLikes(this.pagination.page, this.pagination.limit, this.likerKind === 'sender')
      .subscribe(result => {
        const { items, ...pagination } = result;
        this.users = items;
        this.pagination = pagination;
      }, error => {
        this.alertify.error(error.error);
      }
      );
  }

  pageChanged(event: any): void {
    this.pagination.page = event.page;
    this.loadUsers();
  }

}
