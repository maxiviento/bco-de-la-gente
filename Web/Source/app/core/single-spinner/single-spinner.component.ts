import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';
import { SingleSpinnerService, SingleSpinnerState } from './single-spinner.service';

@Component({
  selector: 'single-spinner',
  templateUrl: './single-spinner.component.html',
  styleUrls: ['./single-spinner.component.scss']
})
export class SingleSpinnerComponent implements OnDestroy, OnInit {
  public visible: boolean = false;
  private currentTimeout: any;
  private spinnerStateChanged: Subscription;

  constructor(private spinnerService: SingleSpinnerService) {
  }

  public ngOnInit() {
    this.spinnerStateChanged = this.spinnerService.spinnerState
      .subscribe((state: SingleSpinnerState) => {
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
        }, 500);

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
