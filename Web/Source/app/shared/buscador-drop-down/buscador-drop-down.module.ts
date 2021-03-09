import { NgModule } from '@angular/core';
import { NgSelectModule } from '../forms/ng-select/ng-select.module';
import { ReactiveFormsModule } from '@angular/forms';
import { BuscadorDropDownComponent } from './buscador-drop-down.component';

@NgModule({
  imports: [
    NgSelectModule,
    ReactiveFormsModule
  ],
  declarations: [
    BuscadorDropDownComponent
  ],
  providers: [],
  exports: [
    NgSelectModule,
    BuscadorDropDownComponent
  ],
  entryComponents: []
})
export class BuscadorDropDownModule {
}
