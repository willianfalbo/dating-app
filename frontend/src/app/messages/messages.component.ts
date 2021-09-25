import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { AlertifyService } from '../_services/alertify.service';
import { MessagesService } from '../_services/messages.service';

import { Message } from '../_models/message';
import { Pagination, Paginated } from '../_models/pagination';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {

  messages: Message[];
  pagination: Pagination;
  messageContainer: any = 'Unread';

  constructor(
    private messagesService: MessagesService,
    private route: ActivatedRoute,
    private alertify: AlertifyService
  ) { }

  ngOnInit() {
    // get data before activating the route. It can be used to avoid using safe navigators "?" in html page
    this.route.data.subscribe(data => {
      const { items, ...pagination } = data['messagesResolver'] as Paginated<Message>;
      this.messages = items;
      this.pagination = pagination;
    });
  }

  loadMessages() {
    this.messagesService
      .getMessages(this.pagination.page, this.pagination.limit, this.messageContainer)
      .subscribe(result => {
        const { items, ...pagination } = result;
        this.messages = items;
        this.pagination = pagination;
      }, error => {
        this.alertify.error(error.error);
      });
  }

  deleteMessage(messageId: number) {
    this.alertify.confirm('Are you sure you want to delete this message?', () => {
      this.messagesService.deleteMessage(messageId).subscribe(() => {
        this.messages.splice(this.messages.findIndex(m => m.id === messageId), 1);
        this.alertify.success('Message has been deleted.');
      }, error => {
        this.alertify.error(error.error);
      });
    });
  }

  pageChanged(event: any): void {
    this.pagination.page = event.page;
    this.loadMessages();
  }

}
