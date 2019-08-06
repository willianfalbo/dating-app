import { Routes } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { MembersComponent } from './members/members.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { AuthGuard } from './_guards/auth.guard';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberDetailResolver } from './members/member-detail/member-detail.resolver';
import { MembersResolver } from './members/members.resolver';

export const ROUTES: Routes = [
    { path: '', component: HomeComponent },
    { path: 'home', component: HomeComponent },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            {
                path: 'members', component: MembersComponent,
                // get data before activating the route.
                // It can be used to avoid using safe navigators "?" in html page
                resolve: { usersResolver: MembersResolver }
            },
            {
                path: 'members/:id', component: MemberDetailComponent,
                // get data before activating the route.
                // It can be used to avoid using safe navigators "?" in html page
                resolve: { userResolver: MemberDetailResolver }
            },
            { path: 'lists', component: ListsComponent },
            { path: 'messages', component: MessagesComponent },
        ]
    },
    // this one must be underneath the list. It can be used for not found pages as well
    { path: '**', redirectTo: '', pathMatch: 'full' },
];
