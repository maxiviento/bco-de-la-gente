import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/distinctUntilChanged';
import { NgSelectComponent } from './ng-select/ng-select.component';
import { NgSelectResultsComponent } from './ng-select-results/ng-select-results.component';
import { CommonModule } from '@angular/common';
import { HighlightPipe } from './highlight-select.pipe';

@NgModule({
  declarations: [
    NgSelectComponent,
    NgSelectResultsComponent,
    HighlightPipe
  ],
  imports: [
    CommonModule,
    FormsModule,
    HttpModule,
    ReactiveFormsModule
  ],
  exports: [
    NgSelectComponent,
    NgSelectResultsComponent,
    HighlightPipe

  ]
})
export class NgSelectModule {
}
