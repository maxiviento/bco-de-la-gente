import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { NuevoMotivoDestinoComponent } from './nuevo-motivo-destino/nuevo-motivo-destino.component';
import { MotivoDestinoRouting } from './motivo-destino.routing';
import { ApartadoMotivoDestinoComponent } from './shared/apartado-motivo-destino/apartado-motivo-destino.component';
import { MotivosDestinoComponent } from './motivos-destino.component';
import { EdicionMotivoDestinoComponent } from './edicion-motivo-destino/edicion-motivo-destino.component';
import { ConsultaMotivoDestinoComponent } from './consulta-motivo-destino/consulta-motivo-destino.component';
import { EliminacionMotivoDestinoComponent } from './eliminacion-motivo-destino/eliminacion-motivo-destino.component';

@NgModule({
  imports: [
    SharedModule,
    MotivoDestinoRouting
  ],
  declarations: [
    EdicionMotivoDestinoComponent,
    ConsultaMotivoDestinoComponent,
    EliminacionMotivoDestinoComponent,
    NuevoMotivoDestinoComponent,
    MotivosDestinoComponent,
    ApartadoMotivoDestinoComponent
  ]
})

export class MotivoDestinoModule {
}
