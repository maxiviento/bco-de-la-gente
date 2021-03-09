import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { ConfiguracionChecklistComponent } from './configuracion-checklist.component';
import { ConfiguracionChecklistRoutingModule } from './configuracion-checklist-routing.module';
import { ConfiguracionChecklistService } from './shared/configuracion-checklist.service';
import { LineaService } from '../shared/servicios/linea-prestamo.service';
import { AreasService } from '../areas/shared/areas.service';
import { EtapasService } from '../etapas/shared/etapas.service';
import { ItemsService } from '../items/shared/items.service';
import { ConfiguracionEtapaEstadoLineaComponent } from './configuracion-etapa-estado-linea/configuracion-etapa-estado-linea.component';

@NgModule({
  imports: [
    SharedModule,
    ConfiguracionChecklistRoutingModule
  ],
  declarations: [
    ConfiguracionChecklistComponent,
    ConfiguracionEtapaEstadoLineaComponent
  ],
  providers: [
    ConfiguracionChecklistService,
    ItemsService,
    EtapasService,
    LineaService,
    AreasService
  ],
  exports: [
    ConfiguracionChecklistRoutingModule
  ]
})

export class ConfiguracionChecklistModule {
}
