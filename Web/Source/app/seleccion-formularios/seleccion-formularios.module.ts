import { SharedModule } from '../shared/shared.module';
import { NgModule } from '@angular/core';
import { BusquedaFormulariosComponent } from './shared/componentes/busqueda-formularios/busqueda-formularios.component';
import { GrillaFormulariosComponent } from './shared/componentes/grilla-formularios/grilla-formularios.component';
import { SeleccionFormulariosComponent } from './seleccion-formularios.component';
import { SeleccionFormulariosRoutingModule } from './seleccion-formularios-routing.module';
import { ActualizarSucursalComponent } from './shared/componentes/actualizar-sucursal/actualizar-sucursal.component';
import { DocumentacionPagosComponent } from './shared/componentes/documentacion-pagos/documentacion-pagos.component';
import { ConsultaSituacionPersonasComponent } from './consulta-situacion-personas/consulta-situacion-personas.component';
import { FiltrosConsultaSituacionPersonasComponent } from './consulta-situacion-personas/filtros-consulta-situacion-personas/filtros-consulta-situacion-personas.component';
import { GrillaPersonasSituacionBgeComponent } from './consulta-situacion-personas/grilla-personas-situacion-bge/grilla-personas-situacion-bge.component';
import { DetalleSituacionPersonaComponent } from './consulta-situacion-personas/detalle-situacion-persona/detalle-situacion-persona.component';
import { SituacionPersonaService } from './shared/situacion-persona.service';
import { GrillaFormulariosSituacionBgeComponent } from './consulta-situacion-personas/detalle-situacion-persona/grilla-formularios-situacion-bge/grilla-formularios-situacion-bge.component';
import { ModalPlanPagosComponent } from './consulta-situacion-personas/detalle-situacion-persona/grilla-formularios-situacion-bge/modal-plan-pagos/modal-plan-pagos.component';
import { PagosModule } from '../pagos/pagos.module';
import { ActualizarFechaPagoComponent } from './shared/componentes/actualizar-fecha-pago/actualizar-fecha-pago.component';
import { ModalVerMotivosRechazoComponent } from './consulta-situacion-personas/detalle-situacion-persona/grilla-formularios-situacion-bge/modal-ver-motivos-rechazo/modal-ver-motivos-rechazo.component';
import { ModalVerHistorialComponent } from './consulta-situacion-personas/detalle-situacion-persona/grilla-formularios-situacion-bge/modal-ver-historial/modal-ver-historial.component';
import { ModalFechaDocumentosComponent } from './shared/componentes/modal-fecha-documentos/modal-fecha-documento.component';
import { SingleSpinnerModule } from '../core/single-spinner/single-spinner.module';

@NgModule({
  imports: [
    SharedModule,
    SeleccionFormulariosRoutingModule,
    PagosModule,
    SingleSpinnerModule
  ],
  declarations: [
    BusquedaFormulariosComponent,
    GrillaFormulariosComponent,
    SeleccionFormulariosComponent,
    ActualizarSucursalComponent,
    DocumentacionPagosComponent,
    ConsultaSituacionPersonasComponent,
    FiltrosConsultaSituacionPersonasComponent,
    GrillaPersonasSituacionBgeComponent,
    DetalleSituacionPersonaComponent,
    GrillaFormulariosSituacionBgeComponent,
    ModalPlanPagosComponent,
    ModalVerMotivosRechazoComponent,
    ModalVerHistorialComponent,
    ActualizarFechaPagoComponent,
    ModalFechaDocumentosComponent
  ],
  providers: [
    SituacionPersonaService,
  ],
  entryComponents: [
    ModalFechaDocumentosComponent
  ]
})

export class SeleccionFormulariosModule {
}
