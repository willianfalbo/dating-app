import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HTTP_INTERCEPTORS } from '@angular/common/http';
import { Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { LoadingScreenService } from './loading-screen.service';

// this interceptor was created based on
// https://nezhar.com/blog/create-a-loading-screen-for-angular-apps/
@Injectable()
export class LoadingScreenInterceptor implements HttpInterceptor {

  activeRequests = 0;

  // URLs for which the loading screen should not be enabled
  skippUrls = [
    // '/authrefresh',
  ];
  constructor(private loadingScreenService: LoadingScreenService) {
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    let displayLoadingScreen = true;

    for (const skippUrl of this.skippUrls) {
      if (new RegExp(skippUrl).test(request.url)) {
        displayLoadingScreen = false;
        break;
      }
    }

    if (displayLoadingScreen) {
      if (this.activeRequests === 0) {
        this.loadingScreenService.startLoading();
      }
      this.activeRequests++;
      return next.handle(request).pipe(
        finalize(() => {
          this.activeRequests--;
          if (this.activeRequests === 0) {
            this.loadingScreenService.stopLoading();
          }
        })
      );
    } else {
      return next.handle(request);
    }
  }
}

export const LoadingScreenInterceptorProvider = {
  provide: HTTP_INTERCEPTORS,
  useClass: LoadingScreenInterceptor,
  multi: true
};
