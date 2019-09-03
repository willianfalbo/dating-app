import { environment } from 'src/environments/environment';

export const DATINGAPP_API_URL = environment.apiUrl;

// https://github.com/auth0/angular2-jwt
// up to this point, using full url does not work when sending up jwt tokens automatically.
// We should format it as the following sample:
// E.g. The url 'https://datingapp-api.azurewebsites.net/api' should be formatted as 'datingapp-api.azurewebsites.net'
export const DATINGAPP_API_HOST_URL = new URL(environment.apiUrl).host;

export const TOKEN_NAME = 'token';

export const USER_OBJECT_NAME = 'user';
