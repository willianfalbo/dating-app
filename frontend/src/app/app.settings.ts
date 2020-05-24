import { JwtModuleOptions } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';

export const DATINGAPP_API_URL = environment.apiUrl;

export const TOKEN_NAME = 'token';

export const USER_OBJECT_NAME = 'user';

export const JWT_MODULE_OPTIONS: JwtModuleOptions = {
    config: {
        tokenGetter: tokenGetter,
        whitelistedDomains: ['localhost:5000', 'datingapp-api.azurewebsites.net'],
        blacklistedRoutes: ['localhost:5000/api/auth', 'datingapp-api.azurewebsites.net/api/auth'] // except auth api
    }
};

export function tokenGetter(): string {
    return localStorage.getItem(TOKEN_NAME);
}
