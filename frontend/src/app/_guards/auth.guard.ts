import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(
    private authService: AuthService,
    private router: Router,
    private alertify: AlertifyService
  ) { }

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if (this.authService.isLoggedIn()) {
      const roles = next.firstChild.data['roles'] as string[];
      if (roles) {
        const match = this.authService.roleMatch(roles);
        if (!match) {
          this.alertify.error('You are not authorized to access this area.');
          this.router.navigate(['members']);
        }
      }
      return true;
    } else {
      this.alertify.error('You must sigin to access this page.');
      this.router.navigate(['/home']);
      return false;
    }
  }
}
