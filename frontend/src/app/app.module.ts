// angular modules
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { JwtModule } from '@auth0/angular-jwt';

// other modules
import { BsDropdownModule, TabsModule, BsDatepickerModule, PaginationModule, ButtonsModule } from 'ngx-bootstrap';
import { NgxGalleryModule } from 'ngx-gallery';
import { FileUploadModule } from 'ng2-file-upload';
import { TimeAgoPipe } from 'time-ago-pipe';

// services / settings
import { AuthService } from './_services/auth.service';
import { AlertifyService } from './_services/alertify.service';
import { ErrorInterceptorProvider } from './_services/error.interceptor';
import { UserService } from './_services/user.service';
import { AuthGuard } from './_guards/auth.guard';
import { MemberEditLeaveGuard } from './members/member-edit/member-edit.leave.guard';
import { LoadingScreenService } from './_services/loading-screen.service';
import { JWT_MODULE_OPTIONS } from './app.settings';

// components
import { AppComponent } from './app.component';
import { ROUTES } from './app.routes';
import { NavComponent } from './nav/nav.component';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { MembersComponent } from './members/members.component';
import { MemberCardComponent } from './members/member-card/member-card.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberDetailResolver } from './members/member-detail/member-detail.resolver';
import { MembersResolver } from './members/members.resolver';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberEditResolver } from './members/member-edit/member-edit.resolver';
import { MemberEditPhotoComponent } from './members/member-edit-photo/member-edit-photo.component';
import { MemberMessagesComponent } from './members/member-messages/member-messages.component';
import { MessagesComponent } from './messages/messages.component';
import { MessagesResolver } from './messages/messages.resolver';
import { ListsComponent } from './lists/lists.component';
import { ListsResolver } from './lists/lists.resolver';
import { LoadingScreenComponent } from './loading-screen/loading-screen.component';
import { LoadingScreenInterceptorProvider } from './_services/loading-screen.interceptor';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { HasRoleDirective } from './_directives/hasRole.directive';

@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      HomeComponent,
      RegisterComponent,
      MembersComponent,
      MemberCardComponent,
      MemberDetailComponent,
      MemberEditComponent,
      MemberEditPhotoComponent,
      MemberMessagesComponent,
      MessagesComponent,
      ListsComponent,
      TimeAgoPipe,
      LoadingScreenComponent,
      AdminPanelComponent,
      HasRoleDirective,
   ],
   imports: [
      BrowserModule,
      BrowserAnimationsModule,
      HttpClientModule,
      FormsModule,
      ReactiveFormsModule,
      BsDropdownModule.forRoot(),
      BsDatepickerModule.forRoot(),
      TabsModule.forRoot(),
      PaginationModule.forRoot(),
      ButtonsModule.forRoot(),
      RouterModule.forRoot(ROUTES),
      JwtModule.forRoot(JWT_MODULE_OPTIONS), // send up jwt tokens automatically (https://github.com/auth0/angular2-jwt)
      NgxGalleryModule,
      FileUploadModule,
   ],
   providers: [
      AuthService,
      ErrorInterceptorProvider,
      AlertifyService,
      AuthGuard,
      UserService,
      MemberDetailResolver,
      MembersResolver,
      MemberEditResolver,
      MemberEditLeaveGuard,
      ListsResolver,
      MessagesResolver,
      LoadingScreenService,
      LoadingScreenInterceptorProvider,
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
