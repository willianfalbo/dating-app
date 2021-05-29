import { Component, OnInit, Input, ViewChild, ElementRef } from '@angular/core';
import { Message } from 'src/app/_models/message';
import { UserService } from 'src/app/_services/user.service';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

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

  constructor(private userService: UserService,
    private authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.loadMessages();
  }

  loadMessages() {
    const userId = +this.authService.decodedToken.userId;
    this.userService.getMessagesThread(userId, this.recipientId)
      .subscribe(messages => {
        this.messages = messages;
        // mark sender's messages as read
        this.userService.markSenderMessagesAsRead(userId, this.recipientId)
          .subscribe(response => { }, error => {
            this.alertify.error(error);
          });
      }, error => {
        this.alertify.error(error);
      });
  }

  sendMessage() {
    this.newMessage.recipientId = this.recipientId;
    this.userService.sendMessage(+this.authService.decodedToken.userId, this.newMessage)
      .subscribe((message: Message) => {
        this.messages.push(message);
        this.newMessage.content = '';
        this.scrollToBottom();
      }, error => {
        this.alertify.error(error);
      });
  }

  scrollToBottom(): void {
    try {
      this.myScrollContainer.nativeElement.scrollTop = this.myScrollContainer.nativeElement.scrollHeight;
    } catch (err) { }
  }

}
