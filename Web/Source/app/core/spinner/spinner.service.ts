import { Injectable } from '@angular/core';
import { Subject } from 'rxjs/Subject';

export interface SpinnerState {
  show: boolean;
}

@Injectable()
export class SpinnerService {
  public spinnerSubject = new Subject<SpinnerState>();
  public spinnerState = this.spinnerSubject.asObservable();

  public show() {
    this.spinnerSubject.next(<SpinnerState>{ show: true });
  }

  public hide() {
    this.spinnerSubject.next(<SpinnerState>{ show: false });
  }
}
