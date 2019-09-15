import { Component, OnInit } from '@angular/core';
import { AuthService } from './_services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  constructor(private authService: AuthService) { }

  ngOnInit(): void {

    // get decoded token
    const token = this.authService.getToken();
    if (token) {
      this.authService.decodedToken = this.authService.getDecodedToken(token);
    }

    // get current user
    const user = this.authService.getUser();
    if (user) {
      this.authService.updateMember(user);
    }

  }

}
