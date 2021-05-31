import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse, HTTP_INTERCEPTORS } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError(error => {
        if (error instanceof HttpErrorResponse) {
          if (error.status === 401) {
            return throwError(error.statusText);
          }
          const applicationError = error.headers.get('Application-Error');
          if (applicationError) {
            console.error(error);
            return throwError(error);
          }
          const serverError = this.getServerErrors(error);
          const modalStateErrors = this.getModalStateErrors(serverError);
          return throwError(modalStateErrors || serverError || 'Server Error');
        }
      })
    );
  }

  private getServerErrors(error: HttpErrorResponse): string[] {
    let serverError: string[];
    if (!error.error.errors) {
      serverError = [error.error];
    } else {
      serverError = error.error.errors;
    }
    return serverError;
  }

  private getModalStateErrors(serverError: string[]): string {
    let modalStateErrors = '';
    if (serverError && typeof serverError === 'object') {
      for (const key in serverError) {
        if (serverError[key]) {
          modalStateErrors += serverError[key] + '\n';
        }
      }
    }
    return modalStateErrors;
  }
}

export const ErrorInterceptorProvider = {
  provide: HTTP_INTERCEPTORS,
  useClass: ErrorInterceptor,
  multi: true
};
