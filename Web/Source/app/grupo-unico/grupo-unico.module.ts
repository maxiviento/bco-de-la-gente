import { NgModule } from '@angular/core';
import { GrupoUnicoService } from './shared-grupo-unico/grupo-unico.service';
import { GrupoUnicoComponent } from './grupo-unico.component';
import { SharedModule } from '../shared/shared.module';
import { GrupoUnicoGaranteComponent } from './grupo-unico-garante.component';
import { GrupoFamiliarService } from '../formularios/shared/grupo-familiar.service';
import { DomicilioService } from './shared-grupo-unico/domicilio.service';

@NgModule({
  imports: [SharedModule],
  declarations: [
    GrupoUnicoComponent,
    GrupoUnicoGaranteComponent
  ],
  providers: [
    GrupoUnicoService,
    GrupoFamiliarService,
    DomicilioService
  ],
  exports: [
    SharedModule,
    GrupoUnicoComponent,
    GrupoUnicoGaranteComponent
  ]
})

export class GrupoUnicoModule {

}
