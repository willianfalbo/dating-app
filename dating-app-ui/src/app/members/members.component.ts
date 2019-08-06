import { Component, OnInit } from '@angular/core';
import { User } from '../_models/user';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-members',
  templateUrl: './members.component.html',
  styleUrls: ['./members.component.css']
})
export class MembersComponent implements OnInit {

  users: User[];

  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      // get data before activating the route.
      // It can be used to avoid using safe navigators "?" in html page
      this.users = data['usersResolver'];
    });
  }

}
