// angular modules
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { JwtModule } from '@auth0/angular-jwt';

// other modules
import { BsDropdownModule, TabsModule } from 'ngx-bootstrap';
import { NgxGalleryModule } from 'ngx-gallery';

// services / settings
import { AuthService } from './_services/auth.service';
import { AlertifyService } from './_services/alertify.service';
import { ErrorInterceptorProvider } from './_services/error.interceptor';
import { UserService } from './_services/user.service';
import { AuthGuard } from './_guards/auth.guard';
import { DATINGAPP_API_URL, TOKEN_NAME } from './app.setting';

// components
import { AppComponent } from './app.component';
import { ROUTES } from './app.routes';
import { NavComponent } from './nav/nav.component';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { MembersComponent } from './members/members.component';
import { MemberCardComponent } from './members/member-card/member-card.component';
import { MessagesComponent } from './messages/messages.component';
import { ListsComponent } from './lists/lists.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberDetailResolver } from './members/member-detail/member-detail.resolver';
import { MembersResolver } from './members/members.resolver';

@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      HomeComponent,
      RegisterComponent,
      MembersComponent,
      MemberCardComponent,
      MemberDetailComponent,
      MessagesComponent,
      ListsComponent,
   ],
   imports: [
      BrowserModule,
      HttpClientModule,
      FormsModule,
      BsDropdownModule.forRoot(),
      TabsModule.forRoot(),
      RouterModule.forRoot(ROUTES),
      // send up jwt tokens automatically (https://github.com/auth0/angular2-jwt)
      JwtModule.forRoot({
         config: {
            tokenGetter: () => {
               return localStorage.getItem(TOKEN_NAME);
            },
            whitelistedDomains: [FormatUrl(DATINGAPP_API_URL)],
            blacklistedRoutes: [`${FormatUrl(DATINGAPP_API_URL)}/api/auth`] // except auth api
         }
      }),
      NgxGalleryModule,
   ],
   providers: [
      AuthService,
      ErrorInterceptorProvider,
      AlertifyService,
      AuthGuard,
      UserService,
      MemberDetailResolver,
      MembersResolver,
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }

// up to this point using full url does not work when sending up jwt tokens automatically. We should format it as the following sample:
// E.g. The url "http://localhost:5000/api" should be formatted as "localhost:5000"
export function FormatUrl(url: string) {
   const formattedUrl = new URL(url).host;
   // console.log('FORMATTED URL', formattedUrl);
   return formattedUrl;
}
