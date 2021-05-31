import { Component, OnInit, Input } from '@angular/core';
import { User } from 'src/app/_models/user';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { LikesService } from 'src/app/_services/likes.service';
import '../../_shared/extensions/string.extensions';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {

  @Input() user: User;

  constructor(
    private likesService: LikesService,
    private alertify: AlertifyService
  ) { }

  ngOnInit() {
  }

  sendLike(recipientId: number) {
    this.likesService.sendLike(recipientId).subscribe(data => {
      this.alertify.success(`You have liked ${this.user.knownAs}.`);
    }, error => {
      if (error.status === 409)
        this.alertify.warning(error.error);
      else
        this.alertify.error(error.error);
    });
  }

}
