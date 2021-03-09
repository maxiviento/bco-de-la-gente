import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';

import { SpinnerState, SpinnerService } from './spinner.service';

@Component({
  selector: 'spinner',
  templateUrl: './spinner.component.html',
  styleUrls: ['./spinner.component.scss']
})
export class SpinnerComponent implements OnDestroy, OnInit {
  public visible: boolean = false;
  private currentTimeout: any;
  private spinnerStateChanged: Subscription;

  constructor(private spinnerService: SpinnerService) {
  }

  public ngOnInit() {
    this.spinnerStateChanged = this.spinnerService.spinnerState
      .subscribe((state: SpinnerState) => {
        if (!state.show) {
          this.cancelTimeout();
          this.visible = state.show;
          return;
        }

        if (this.currentTimeout) {
          return;
        }

        this.currentTimeout = setTimeout(() => {
          this.visible = state.show;
          this.cancelTimeout();
        }, 250);

      });
  }

  public ngOnDestroy() {
    this.spinnerStateChanged.unsubscribe();
    this.cancelTimeout();
  }

  private cancelTimeout(): void {
    clearTimeout(this.currentTimeout);
    this.currentTimeout = undefined;
  }
}
