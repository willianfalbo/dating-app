import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { MemberEditComponent } from './member-edit.component';

@Injectable()
// prevent unsaved changes
export class MemberEditLeaveGuard implements CanDeactivate<MemberEditComponent>{

    constructor() { }

    canDeactivate(component: MemberEditComponent): boolean {
        if (component.editForm.dirty) {
            return confirm('Are you sure you want to continue? Any unsaved changes will be lost!');
        } else {
            return true;
        }
    }

}
