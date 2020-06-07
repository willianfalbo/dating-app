// angular modules
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { JwtModule } from '@auth0/angular-jwt';

// third party modules
import { BsDropdownModule, TabsModule, BsDatepickerModule, PaginationModule, ButtonsModule, ModalModule } from 'ngx-bootstrap';
import { NgxGalleryModule } from 'ngx-gallery';
import { FileUploadModule } from 'ng2-file-upload';
import { TimeAgoPipe } from 'time-ago-pipe';

// settings / guard / providers / directives
import { ROUTES } from './app.routes';
import { JWT_MODULE_OPTIONS } from './app.settings';
import { AuthGuard } from './_guards/auth.guard';
import { MemberEditLeaveGuard } from './members/member-edit/member-edit.leave.guard';
import { ErrorInterceptorProvider } from './_services/error.interceptor';
import { LoadingScreenInterceptorProvider } from './_services/loading-screen.interceptor';
import { HasRoleDirective } from './_directives/hasRole.directive';

// services
import { AuthService } from './_services/auth.service';
import { AlertifyService } from './_services/alertify.service';
import { UserService } from './_services/user.service';
import { LoadingScreenService } from './_services/loading-screen.service';
import { AdminService } from './_services/admin-service';

// resolvers
import { MemberDetailResolver } from './members/member-detail/member-detail.resolver';
import { MembersResolver } from './members/members.resolver';
import { MemberEditResolver } from './members/member-edit/member-edit.resolver';
import { MessagesResolver } from './messages/messages.resolver';
import { ListsResolver } from './lists/lists.resolver';

// components
import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { MembersComponent } from './members/members.component';
import { MemberCardComponent } from './members/member-card/member-card.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberEditPhotoComponent } from './members/member-edit-photo/member-edit-photo.component';
import { MemberMessagesComponent } from './members/member-messages/member-messages.component';
import { MessagesComponent } from './messages/messages.component';
import { ListsComponent } from './lists/lists.component';
import { LoadingScreenComponent } from './loading-screen/loading-screen.component';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { UserManagementComponent } from './admin/user-management/user-management.component';
import { PhotoManagementComponent } from './admin/photo-management/photo-management.component';
import { RolesModalComponent } from './admin/roles-modal/roles-modal.component';

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
    UserManagementComponent,
    PhotoManagementComponent,
    RolesModalComponent,
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
    ModalModule.forRoot(),
  ],
  providers: [
    ErrorInterceptorProvider,
    LoadingScreenInterceptorProvider,
    AuthGuard,
    MemberEditLeaveGuard,
    AuthService,
    AlertifyService,
    UserService,
    LoadingScreenService,
    AdminService,
    MemberDetailResolver,
    MembersResolver,
    MemberEditResolver,
    ListsResolver,
    MessagesResolver,
  ],
  entryComponents: [
    RolesModalComponent
  ],
  bootstrap: [
    AppComponent
  ]
})
export class AppModule { }
