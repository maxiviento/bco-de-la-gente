import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { ConfiguracionFormularioModule } from './configuracion-formulario/configuracion-formulario.module';
import { NuevaLineaComponent } from './nueva-linea/nueva-linea.component';
import { LineaItemsComponent } from './shared/linea-items/linea-items.component';
import { LineaDetalleComponent } from './shared/linea-detalle/linea-detalle.component';
import { LineaDatosSeleccionadosComponent } from './shared/linea-datos-seleccionados/linea-datos-seleccionados.component';
import { LineaRequisitosComponent } from './shared/requisitos/linea-requisitos/linea-requisitos.component';
import { TipoRequisitoComponent } from './shared/requisitos/linea-requisitos/tipo-requisito/tipo-requisito.component';
import { RequisitoComponent } from './shared/requisitos/linea-requisitos/tipo-requisito/requisito/requisito.component';
import { ConsultaLineaComponent } from './consulta-linea/consulta-linea.component';
import { LineasPrestamoComponent } from './lineas-prestamo.component';
import { ConsultaRequisitosModal } from './shared/requisitos/consulta-requisitos/consulta-requisitos.component';
import { LineaArchivosModal } from './shared/linea-archivos-modal/linea-archivos.modal';
import { EliminacionLineaComponent } from './eliminacion-linea/eliminacion-linea.component';
import { LineaRoutingModule } from './lineas-prestamo-routing.module';
import { LineaService } from '../shared/servicios/linea-prestamo.service';
import { DestinatarioService } from './shared/destinatario.service';
import { IntegrantesService } from './shared/integrantes.service';
import { MotivoDestinoService } from '../motivo-destino/shared/motivo-destino.service';
import { TiposFinanciamientoService } from './shared/tipos-financiamiento.service';
import { TiposInteresService } from './shared/tipos-interes.service';
import { TiposGarantiaService } from './shared/tipos-garantia.service';
import { ParametroService } from '../soporte/parametro.service';
import { ConsultaDetalleLineaComponent } from './consulta-linea/consulta-detalle-linea/consulta-detalle-linea.component';
import { EdicionLineaComponent } from './edicion-linea/edicion-linea.component';
import { ModalDetalleLineaComponent } from './modal-detalle-linea/modal-detalle-linea.component';
import { EliminacionDetalleLineaComponent } from './eliminacion-linea/eliminacion-detalle-linea/eliminacion-detalle-linea.component';
import { EdicionDetalleLineaComponent } from './edicion-linea/edicion-detalle-linea/edicion-detalle-linea.component';
import { ProgramaService } from './shared/ProgramaService';
import { LineasOngComponent } from './shared/linea-ong/lineas-ong.component';
import { ConsultaOngLineaModal } from './shared/linea-ong/consulta-ong-linea/consulta-ong-linea.component';

@NgModule({
  imports: [
    SharedModule,
    ConfiguracionFormularioModule
  ],
  declarations: [
    NuevaLineaComponent,
    LineaItemsComponent,
    LineaDetalleComponent,
    LineaDatosSeleccionadosComponent,
    LineaRequisitosComponent,
    LineasOngComponent,
    TipoRequisitoComponent,
    RequisitoComponent,
    ConsultaLineaComponent,
    LineasPrestamoComponent,
    ConsultaRequisitosModal,
    LineaArchivosModal,
    EliminacionLineaComponent,
    LineaArchivosModal,
    EdicionLineaComponent,
    ConsultaDetalleLineaComponent,
    ModalDetalleLineaComponent,
    EdicionDetalleLineaComponent,
    EliminacionDetalleLineaComponent,
    ConsultaOngLineaModal
  ],
  exports: [
    LineaRoutingModule
  ],
  providers: [
    LineaService,
    DestinatarioService,
    IntegrantesService,
    MotivoDestinoService,
    TiposFinanciamientoService,
    TiposInteresService,
    TiposGarantiaService,
    ParametroService,
    ProgramaService
  ],

  entryComponents: [
    ConsultaRequisitosModal,
    ModalDetalleLineaComponent,
    LineaArchivosModal,
    ConsultaOngLineaModal
  ]
})

export class LineaModule {
}
