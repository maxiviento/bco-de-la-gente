import { NgModule } from '@angular/core';
import { BandejaPagosComponent } from './bandeja-pagos/bandeja-pagos.component';
import { SharedModule } from '../shared/shared.module';
import { LocalidadComboServicio } from '../formularios/shared/localidad.service';
import { OrigenService } from '../formularios/shared/origen-prestamo.service';
import { PagosRoutingModule } from './pagos-routing.module';
import { FormulariosService } from '../formularios/shared/formularios.service';
import { PagosService } from './shared/pagos.service';
import { BandejaLotesComponent } from './bandeja-lotes/bandeja-lotes.component';
import { ModalImpresionMicroprestamosComponent } from './shared/modal-impresion-microprestamos/modal-impresion-microprestamos.component';
import { DetalleLoteComponent } from './detalle-lote/detalle-lote.component';
import { DetalleLoteVerComponent } from './detalle-lote-ver/detalle-lote-ver.component';
import { DetalleLoteDesagruparComponent } from './detalle-lote-desagrupar/detalle-lote-desagrupar.component';
import { DetalleLoteLiberarComponent } from './detalle-lote-liberar/detalle-lote-liberar.component';
import { ArmarLoteSuafComponent } from './suaf/armar-lote-suaf/armar-lote-suaf.component';
import { PlanPagosComponent } from './plan-pagos/plan-pagos.component';
import { ActualizarPlanPagosComponent } from './plan-pagos/actualizar-plan-pagos/actualizar-plan-pagos.component';
import { GrillaPlanPagosComponent } from './plan-pagos/actualizar-plan-pagos/grilla-plan-pagos/grilla-plan-pagos.component';
import { FiltrosFormulariosPlanPagosComponent } from './plan-pagos/filtros-formularios-plan-pagos/filtros-formularios-plan-pagos.component';
import { GrillaFormulariosPlanPagosComponent } from './plan-pagos/grilla-formularios-plan-pagos/grilla-formularios-plan-pagos.component';
import { BandejaSuafComponent } from './suaf/bandeja-suaf/bandeja-suaf.component';
import { BandejaFormulariosSuafComponent } from './suaf/bandeja-formularios-suaf/bandeja-formularios-suaf.component';
import { CargaManualDevengadoComponent } from './suaf/carga-manual-devengado/carga-manual-devengado.component';
import { ModalNotaPagosComponent } from './modal-nota-pagos/modal-nota-pagos.component';
import { BandejaRecuperoComponent } from '../recupero/bandeja-recupero/bandeja-recupero.component';
import { RecuperoService } from '../recupero/shared/recupero.service';
import { VerLoteSuafComponent } from './suaf/ver-lote-suaf/ver-lote-suaf.component';
import { VisualizarPlanPagosComponent } from './plan-pagos/visualizar-plan-pagos/visualizar-plan-pagos.component';
import { BandejaResultadoBancoComponent } from '../recupero/bandeja-resultado-banco/bandeja-resultado-banco.component';
import { VerInconsistenciaRecuperoComponent } from '../recupero/ver-inconsistencia-recupero/ver-inconsistencia-recupero.component';
import { VerInconsistenciaResultadoComponent } from '../recupero/ver-inconsistencia-resultado/ver-inconsistencia-resultado.component';
import { ModalTipoEntidadComponent } from '../recupero/modal-tipo-entidad/modal-tipo-entidad.component';
import { GrillaBandejaSuafComponent } from './suaf/bandeja-suaf/grilla-bandeja-suaf/grilla-bandeja-suaf.component';
import { SingleSpinnerModule } from '../core/single-spinner/single-spinner.module';
import { ConsultaLoteSuafComponent } from './suaf/bandeja-suaf/consulta-lote-suaf/consulta-lote-suaf.component';
import { ConsultaBandejaLotesComponent } from './bandeja-lotes/consulta-bandeja-lotes/consulta-bandeja-lotes.component';
import { GrillaBandejaLotesComponent } from './bandeja-lotes/grilla-bandeja-lotes/grilla-bandeja-lotes.component';
import { ModalidadPagoComponent } from './modalidad-pago/modalidad-pago.component';
import { ModalModalidadPagoComponent } from './modal-modalidad-pago/modal-modalidad-pago.component';
import { ModalFechaProvidenciaComponent } from './modal-fecha-providencia/modal-fecha-providencia.component';
import { ModalAdendaComponent } from './bandeja-lotes/modal-adenda/modal-adenda.component';
import { GenerarLotePagoComponent } from './suaf/generar-lote-pago/generar-lote-pago.component';
import { BandejaCambioEstadoComponent } from './bandeja-cambio-estado/bandeja-cambio-estado.component';
import { ConsultaBandejaCambioEstadoComponent } from './bandeja-cambio-estado/consulta-grilla-cambio-estado/consulta-bandeja-cambio-estado.component';
import { GrillaBandejaCambioEstadoComponent } from './bandeja-cambio-estado/grilla-bandeja-cambio-estado/grilla-bandeja-cambio-estado.component';
import { ConsultaBandejaChequeComponent } from './bandeja-formularios-cheque/consulta-bandeja-cheques/consulta-bandeja-cheque.component';
import { GrillaBandejaChequesComponent } from './bandeja-formularios-cheque/grilla-bandeja-cheques/grilla-bandeja-cheques.component';
import { BandejaChequesComponent } from './bandeja-formularios-cheque/bandeja-cheques.component';
import { DetalleLoteAgregarPrestamoComponent } from './detalle-lote-agrupar/detalle-lote-agregar-prestamo.component';
import { BandejaAgregarPrestamoComponent } from './detalle-lote-agrupar/consulta-prestamos/bandeja-agregar-prestamo.component';
import { ModalTipoPagosComponent } from './modal-tipo-pagos/modal-tipo-pagos.component';
import {BandejaCrearAdendaComponent} from "./bandeja-crear-adenda/bandeja-crear-adenda.component";
import {ConsultaCrearAdendaComponent} from "./bandeja-crear-adenda/consulta-crear-adenda-estado/consulta-crear-adenda.component";

@NgModule({
  imports: [
    SharedModule,
    SingleSpinnerModule
  ],
  declarations: [
    BandejaPagosComponent,
    ModalImpresionMicroprestamosComponent,
    BandejaLotesComponent,
    DetalleLoteComponent,
    DetalleLoteVerComponent,
    DetalleLoteDesagruparComponent,
    DetalleLoteLiberarComponent,
    PlanPagosComponent,
    ActualizarPlanPagosComponent,
    GrillaPlanPagosComponent,
    FiltrosFormulariosPlanPagosComponent,
    GrillaFormulariosPlanPagosComponent,
    ArmarLoteSuafComponent,
    BandejaSuafComponent,
    BandejaFormulariosSuafComponent,
    CargaManualDevengadoComponent,
    VerLoteSuafComponent,
    GenerarLotePagoComponent,
    ModalNotaPagosComponent,
    ModalFechaProvidenciaComponent,
    VisualizarPlanPagosComponent,
    BandejaRecuperoComponent,
    BandejaResultadoBancoComponent,
    VerInconsistenciaRecuperoComponent,
    VerInconsistenciaResultadoComponent,
    ModalTipoEntidadComponent,
    GrillaBandejaSuafComponent,
    ConsultaLoteSuafComponent,
    ConsultaBandejaLotesComponent,
    GrillaBandejaLotesComponent,
    ModalidadPagoComponent,
    ModalModalidadPagoComponent,
    ModalAdendaComponent,
    ConsultaBandejaChequeComponent,
    GrillaBandejaChequesComponent,
    BandejaChequesComponent,
    BandejaCambioEstadoComponent,
    BandejaCrearAdendaComponent,
    ConsultaCrearAdendaComponent,
    ConsultaBandejaCambioEstadoComponent,
    GrillaBandejaCambioEstadoComponent,
    DetalleLoteAgregarPrestamoComponent,
    BandejaAgregarPrestamoComponent,
    ModalTipoPagosComponent
  ],
  exports: [
    BandejaPagosComponent,
    PagosRoutingModule,
    BandejaLotesComponent,
    DetalleLoteComponent,
    DetalleLoteVerComponent,
    DetalleLoteDesagruparComponent,
    DetalleLoteLiberarComponent,
    DetalleLoteAgregarPrestamoComponent,
    BandejaAgregarPrestamoComponent,
    ArmarLoteSuafComponent,
    BandejaSuafComponent,
    GrillaPlanPagosComponent,
    BandejaFormulariosSuafComponent,
    CargaManualDevengadoComponent,
    VerLoteSuafComponent,
    GenerarLotePagoComponent,
    ModalNotaPagosComponent,
    ModalFechaProvidenciaComponent,
    BandejaRecuperoComponent,
    BandejaResultadoBancoComponent,
    VerInconsistenciaRecuperoComponent,
    VerInconsistenciaResultadoComponent,
    ModalTipoEntidadComponent,
    ModalidadPagoComponent,
    ModalModalidadPagoComponent,
    ModalAdendaComponent,
    BandejaCrearAdendaComponent,
    ConsultaCrearAdendaComponent,
    BandejaCambioEstadoComponent,
    ConsultaBandejaCambioEstadoComponent,
    GrillaBandejaCambioEstadoComponent,
    BandejaChequesComponent,
    ModalTipoPagosComponent
  ],
  providers: [
    LocalidadComboServicio,
    OrigenService,
    FormulariosService,
    PagosService,
    RecuperoService
  ],
  entryComponents: [
    ModalImpresionMicroprestamosComponent,
    ModalNotaPagosComponent,
    ModalTipoEntidadComponent,
    ModalFechaProvidenciaComponent,
    ModalModalidadPagoComponent,
    ModalAdendaComponent,
    ModalTipoPagosComponent
  ],
})

export class PagosModule {
}
