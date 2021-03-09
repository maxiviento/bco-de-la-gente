import { MontoDisponibleComponent } from './monto-disponible.component';
import { SharedModule } from '../../shared/shared.module';
import { NgModule } from '@angular/core';
import { MontoDisponibleRoutingModule } from './monto-disponible.routing.module';
import { NuevoMontoDisponibleComponent } from './nuevo-monto-disponible/nuevo-monto-disponible.component';
import { BajaMontoDisponibleComponent } from './baja-monto-disponible/baja-monto-disponible.component';
import { EdicionMontoDisponibleComponent } from './edicion-monto-disponible/edicion-monto-disponible.component';
import { BancoService } from '../../shared/servicios/banco.service';
import { ConsultaMontoDisponibleComponent } from './consulta-monto-disponible/consulta-monto-disponible.component';
import { MontoDisponibleService } from './shared/monto-disponible.service';
import { ModalFormulariosPrestamoComponent } from '../modal-formularios-prestamo/modal-formularios-prestamo.component';

@NgModule({
  imports: [
    SharedModule,
  ],
  declarations: [
    MontoDisponibleComponent,
    NuevoMontoDisponibleComponent,
    BajaMontoDisponibleComponent,
    EdicionMontoDisponibleComponent,
    ConsultaMontoDisponibleComponent,
    ModalFormulariosPrestamoComponent,
  ],
  exports: [
    MontoDisponibleRoutingModule,
    NuevoMontoDisponibleComponent,
    BajaMontoDisponibleComponent,
    EdicionMontoDisponibleComponent,
    ConsultaMontoDisponibleComponent,
    ModalFormulariosPrestamoComponent,
  ],
  providers: [
    BancoService,
    MontoDisponibleService],
  entryComponents: [
    NuevoMontoDisponibleComponent,
    BajaMontoDisponibleComponent,
    EdicionMontoDisponibleComponent,
    ConsultaMontoDisponibleComponent,
    ModalFormulariosPrestamoComponent,
  ],
})

export class MontoDisponibleModule {

}
