import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

// this service was created based on
// https://nezhar.com/blog/create-a-loading-screen-for-angular-apps/
@Injectable({
  providedIn: 'root'
})
export class LoadingScreenService {

  constructor() { }

  private _loading = false;
  loadingStatus: Subject<boolean> = new Subject();

  get loading(): boolean {
    return this._loading;
  }

  set loading(value) {
    this._loading = value;
    this.loadingStatus.next(value);
  }

  startLoading() {
    this.loading = true;
  }

  stopLoading() {
    this.loading = false;
  }

}
