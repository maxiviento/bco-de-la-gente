import { Directive, Input, OnInit, HostBinding, OnChanges, SimpleChanges } from '@angular/core';
import { AbstractControl } from '@angular/forms';
import { Subscription } from 'rxjs';
@Directive({
  selector: '[errorFeedback]'
})
export class ErrorFeedbackDirective implements OnInit, OnChanges {

  @Input('errorFeedback') control: AbstractControl;
  private statusChangesSubscription: Subscription;
  @HostBinding('class.form-control-danger') controlWithError: boolean = false;

  constructor() {
  }

  public ngOnInit(): void {
    this.inicializarControl();
  }

  public ngOnChanges(changes: SimpleChanges): void {
    if (changes.control.currentValue) {
      this.inicializarControl();
    }
  }

  private inicializarControl(): void {
    this.statusChangesSubscription =
      this.control
        .statusChanges
        .subscribe(() => {
          this.controlWithError = (this.control.invalid && this.control.dirty);
        });
  }
}
