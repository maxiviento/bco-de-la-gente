import { SingleSpinnerService } from './single-spinner.service';
import { CommonModule } from '@angular/common';
import { SingleSpinnerComponent } from './single-spinner.component';
import { NgModule } from '@angular/core';

@NgModule({
  imports: [
    CommonModule
  ],
  exports: [SingleSpinnerComponent],
  declarations: [SingleSpinnerComponent],
  providers: [
    SingleSpinnerService
  ]
})
export class SingleSpinnerModule {
}
