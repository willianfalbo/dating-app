import { Component, OnInit, Input } from '@angular/core';
import { User } from 'src/app/_models/user';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import '../../_shared/extensions/string.extensions';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {

  @Input() user: User;

  constructor(
    private userService: UserService,
    private alertify: AlertifyService
  ) { }

  ngOnInit() {
  }

  sendLike(recipientId: number) {
    this.userService.sendLike(recipientId).subscribe(data => {
      this.alertify.success(`You have liked ${this.user.knownAs}.`);
    }, error => {
      this.alertify.error(error);
    });
  }

}
