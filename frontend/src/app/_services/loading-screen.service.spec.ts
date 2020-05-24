/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { LoadingScreenService } from './loading-screen.service';

describe('Service: LoadingScreen', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [LoadingScreenService]
    });
  });

  it('should ...', inject([LoadingScreenService], (service: LoadingScreenService) => {
    expect(service).toBeTruthy();
  }));
});
