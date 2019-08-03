import { Routes } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { MatchesComponent } from './matches/matches.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { AuthGuard } from './_guards/auth.guard';

export const ROUTES: Routes = [
    { path: '', component: HomeComponent },
    { path: 'home', component: HomeComponent },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'matches', component: MatchesComponent },
            { path: 'lists', component: ListsComponent },
            { path: 'messages', component: MessagesComponent },
        ]
    },
    // this one must be underneath the list. It can be used for not found pages as well
    { path: '**', redirectTo: '', pathMatch: 'full' },
];
