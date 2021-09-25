import { Component, OnInit, Input, ViewChild, ElementRef } from '@angular/core';
import { Message } from 'src/app/_models/message';

import { AlertifyService } from 'src/app/_services/alertify.service';
import { MessagesService } from 'src/app/_services/messages.service';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {

  @Input() recipientId: number;
  messages: Message[];
  newMessage: any = {};
  @ViewChild('cardBody') private myScrollContainer: ElementRef;

  constructor(
    private messagesService: MessagesService,
    private alertify: AlertifyService
  ) { }

  ngOnInit() {
    this.loadMessages();
  }

  loadMessages() {
    this.messagesService.getMessagesThread(this.recipientId)
      .subscribe(response => {
        this.messages = response.items;
        // mark sender's messages as read
        this.messagesService.markSenderMessagesAsRead(this.recipientId)
          .subscribe(response => { }, error => {
            this.alertify.error(error.error);
          });
      }, error => {
        this.alertify.error(error.error);
      });
  }

  sendMessage() {
    this.newMessage.recipientId = this.recipientId;
    this.messagesService.sendMessage(this.newMessage)
      .subscribe((message: Message) => {
        this.messages.push(message);
        this.newMessage.content = '';
        this.scrollToBottom();
      }, error => {
        this.alertify.error(error.error);
      });
  }

  scrollToBottom(): void {
    try {
      this.myScrollContainer.nativeElement.scrollTop = this.myScrollContainer.nativeElement.scrollHeight;
    } catch (err) { }
  }

}
