import { NgModule } from '@angular/core';
import { EtapasService } from './shared/etapas.service';
import { EtapasRoutingModule } from './etapas-routing.module';
import { EtapasComponent } from './etapas.component';
import { SharedModule } from '../shared/shared.module';
import { MotivosBajaService } from '../shared/servicios/motivosbaja.service';
import { ConsultaEtapaComponent } from "./consulta-etapa/consulta-etapa.component";
import { EdicionEtapaComponent } from "./edicion-etapa/edicion-etapa.component";
import { EliminacionEtapaComponent } from "./eliminacion-etapa/eliminacion-etapa.component";
import { NuevaEtapaComponent } from "./nueva-etapa/nueva-etapa.component";

@NgModule({
  imports: [
    SharedModule,
    EtapasRoutingModule
  ],
  exports: [EtapasRoutingModule],
  declarations: [EtapasComponent, ConsultaEtapaComponent, EdicionEtapaComponent, EliminacionEtapaComponent, NuevaEtapaComponent],
  providers: [EtapasService, MotivosBajaService]
})

export class EtapasModule {

}
