import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export interface SingleSpinnerState {
  show: boolean;
}

@Injectable()
export class SingleSpinnerService {
  public spinnerSubject
    = new BehaviorSubject<SingleSpinnerState>(<SingleSpinnerState>{show: false});
  public spinnerState = this.spinnerSubject.asObservable().share();

  public show() {
    this.spinnerSubject.next(<SingleSpinnerState>{show: true});
  }

  public hide() {
    this.spinnerSubject.next(<SingleSpinnerState>{show: false});
  }
}
