/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { AdminServiceService } from './admin-service';

describe('Service: AdminService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AdminServiceService]
    });
  });

  it('should ...', inject([AdminServiceService], (service: AdminServiceService) => {
    expect(service).toBeTruthy();
  }));
});
