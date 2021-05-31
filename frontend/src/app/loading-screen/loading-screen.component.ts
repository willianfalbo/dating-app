import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { LoadingScreenService } from '../_services/loading-screen.service';
import { debounceTime } from 'rxjs/operators';

// this component was created based on
// https://nezhar.com/blog/create-a-loading-screen-for-angular-apps/
@Component({
  selector: 'app-loading-screen',
  templateUrl: './loading-screen.component.html',
  styleUrls: ['./loading-screen.component.css']
})
export class LoadingScreenComponent implements OnInit, OnDestroy {

  loading = false;
  loadingSubscription: Subscription;

  constructor(private loadingScreenService: LoadingScreenService) {
  }

  ngOnInit() {
    this.loadingSubscription = this.loadingScreenService.loadingStatus.pipe(
      debounceTime(200)
    ).subscribe((value) => {
      this.loading = value;
    });
  }

  ngOnDestroy() {
    this.loadingSubscription.unsubscribe();
  }

}
