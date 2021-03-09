import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { FormularioComponent } from './formularios.component';
import { CursoComponent } from './cuadrantes/cursos/cursos.component';
import { ContenedorCuadrantesDirective } from './cuadrantes/contenedor-cuadrantes.directive';
import { Linea2PieComponent } from './cuadrantes/pies/linea2-pie.component';
import { CondicionesSolicitadasComponent } from './cuadrantes/condiciones-solicitadas/condiciones-solicitadas.component';
import { DestinoFondosComponent } from './cuadrantes/destino-fondos/destino-fondos.component';
import { BusquedaPersonaComponent } from './cuadrantes/persona/persona.component';
import { SolicitanteComponent } from './cuadrantes/solicitante/solicitante.component';
import { GaranteComponent } from './cuadrantes/garante/garante.component';
import { BandejaFormularioComponent } from './bandeja-formulario/bandeja-formulario.component';
import { EncabezadoComponent } from './cuadrantes/encabezado/encabezado.component';
import { ModalMotivoBajaComponent } from '../shared/modal-motivo-baja/modal-motivo-baja.component';
import { FormularioRoutingModule } from './formularios-routing.module';
import { FormulariosService } from './shared/formularios.service';
import { CursosService } from './cuadrantes/cursos/cursos.service';
import { DestinoFondosService } from './cuadrantes/destino-fondos/destino-fondos.service';
import { LocalidadComboServicio } from './shared/localidad.service';
import { OrigenService } from './shared/origen-prestamo.service';
import { EstadoFormularioService } from './shared/estado-formulario.service';
import { LineaService } from '../shared/servicios/linea-prestamo.service';
import { PersonaService } from './cuadrantes/persona/persona.service';
import { SexoService } from '../shared/servicios/sexo.service';
import { PaisService } from '../shared/servicios/pais.service';
import { GrupoUnicoComponent } from '../grupo-unico/grupo-unico.component';
import { SeleccionLineaComponent } from './seleccion-linea/seleccion-linea.component';
import { ModalSeleccionDetalleComponent } from './seleccion-linea/modal-seleccion-detalle/modal-seleccion-detalle.component';
import { ModalVolverFormularioComponent } from './modal-volver/modal-volver-formulario.component';
import { ValidacionFormularioServicio } from './shared/validacion-formulario.service';
import { GrupoUnicoGaranteComponent } from '../grupo-unico/grupo-unico-garante.component';
import { ModalMotivoRechazoComponent } from './modal-motivo-rechazo/modal-motivo-rechazo.component';
import { MotivoRechazoService } from './shared/motivo-rechazo.service';
import { GrupoFamiliarSolicitanteComponent } from './cuadrantes/grupo-familiar-solicitante/grupo-familiar-solicitante.component';
import { GrupoFamiliarGaranteComponent } from './cuadrantes/grupo-familiar-garante/grupo-familiar-garante.component';
import { CuadranteRequisitosComponent } from './cuadrantes/requisitos/cuadrante-requisitos.component';
import { ReporteFormularioComponent } from './reporte/reporte-formulario.component';
import { ReporteFormularioLineaComponent } from './reporte/reporte-formulario-linea.component';
import { PatrimonioSolicitanteComponent } from './cuadrantes/patrimonio-solicitante/patrimonio-solicitante.component';
import { DatosEmprendimientoComponent } from './cuadrantes/datos-emprendimiento/datos-emprendimiento.component';
import { EmprendimientoService } from './shared/emprendimiento.service';
import { ModalDomicilioComponent } from './cuadrantes/datos-emprendimiento/modal-domicilio/modal-domicilio.component';
import { OrganizacionIndividualComponent } from './cuadrantes/organizacion-individual/organizacion-individual.component';
import { ModalIntegranteEmprendimientoComponent } from './modal-integrante-emprendimiento/modal-integrante-emprendimiento.component';
import { MercadoYComercializacionComponent } from './cuadrantes/mercado-y-comercializacion/mercado-y-comercializacion.component';
import { InversionRealizadaComponent } from './cuadrantes/inversion-realizada/inversion-realizada.component';
import { DescripcionProyectoComponent } from './cuadrantes/descripcion-proyecto/descripcion-proyecto.component';
import { PrecioVentaComponent } from './cuadrantes/precio-venta/precio-venta.component';
import { ResultadoEstimadoMensualComponent } from './cuadrantes/resultado-estimado-mensual/resultado-estimado-mensual.component';
import { ApartadoInversionesEmprendimientoComponent } from './shared/apartado-inversiones-emprendimiento/apartado-inversiones-emprendimiento.component';
import { NecesidadesInversionComponent } from './cuadrantes/necesidades-inversion/necesidades-inversion.component';
import { DeudaEmprendimientoComponent } from './cuadrantes/deuda-emprendimiento/deuda-emprendimiento.component';
import { IngresosYGastosActualesComponent } from './cuadrantes/ingresos-y-gastos-actuales/ingresos-y-gastos-actuales.component';
import { ApartadoAlertaEmprendimientoRequeridoComponent } from './shared/apartado-alerta-emprendimiento-requerido/apartado-alerta-emprendimiento-requerido.component';
import { ModalPlanPagosComponent } from '../seleccion-formularios/consulta-situacion-personas/detalle-situacion-persona/grilla-formularios-situacion-bge/modal-plan-pagos/modal-plan-pagos.component';
import { ContenidoNuevaPersonaComponent } from './contenido-nueva-persona/contenido-nueva-persona.component';
import { GrupoUnicoModule } from '../grupo-unico/grupo-unico.module';
import { IntegrantesPersonasComponent } from './cuadrantes/integrantes-personas/integrantes-personas.component';
import { IntegrantesGrillaComponent } from './cuadrantes/integrantes-grilla/integrantes-grilla.component';
import { ModalModificarGrupoIntegranteComponent } from './modal-modificar-grupo-integrante/modal-modificar-grupo-integrante.component';
import { ActualizacionMasivaCuadrantesFormulariosService } from './shared/actualizacion-masiva-cuadrantes-formularios.service';
import { ONGComponent } from './cuadrantes/ong/ong.component';
import { ModalEditarNumeroCajaComponent } from './modal-editar-numero-caja/modal-editar-numero-caja.component';
import { ContenidoEditarPersonaComponent } from './contenido-editar-persona/contenido-editar-persona.component';
import { ModalVerMotivosRechazoComponent } from '../seleccion-formularios/consulta-situacion-personas/detalle-situacion-persona/grilla-formularios-situacion-bge/modal-ver-motivos-rechazo/modal-ver-motivos-rechazo.component';
import { ModalVerHistorialComponent } from '../seleccion-formularios/consulta-situacion-personas/detalle-situacion-persona/grilla-formularios-situacion-bge/modal-ver-historial/modal-ver-historial.component';
import { SingleSpinnerModule } from '../core/single-spinner/single-spinner.module';
import { ActualizarDatosComponent } from './actualizar-datos/actualizar-datos.component';
import { ActualizarCondicionesSolicitadasComponent } from './actualizar-datos/actualizar-condiciones-solicitadas/actualizar-condiciones-solicitadas.component';

@NgModule({
  imports: [
    SharedModule,
    GrupoUnicoModule,
    SingleSpinnerModule,
  ],
  declarations: [
    FormularioComponent,
    CursoComponent,
    ContenedorCuadrantesDirective,
    Linea2PieComponent,
    CondicionesSolicitadasComponent,
    DestinoFondosComponent,
    BusquedaPersonaComponent,
    SolicitanteComponent,
    IntegrantesPersonasComponent,
    IntegrantesGrillaComponent,
    GaranteComponent,
    BandejaFormularioComponent,
    SeleccionLineaComponent,
    ModalSeleccionDetalleComponent,
    EncabezadoComponent,
    GrupoFamiliarSolicitanteComponent,
    GrupoFamiliarGaranteComponent,
    ModalMotivoBajaComponent,
    ModalVolverFormularioComponent,
    ModalMotivoRechazoComponent,
    ModalEditarNumeroCajaComponent,
    CuadranteRequisitosComponent,
    ReporteFormularioComponent,
    ReporteFormularioLineaComponent,
    PatrimonioSolicitanteComponent,
    OrganizacionIndividualComponent,
    DatosEmprendimientoComponent,
    ModalDomicilioComponent,
    ModalIntegranteEmprendimientoComponent,
    DescripcionProyectoComponent,
    MercadoYComercializacionComponent,
    PrecioVentaComponent,
    IngresosYGastosActualesComponent,
    InversionRealizadaComponent,
    ResultadoEstimadoMensualComponent,
    ApartadoInversionesEmprendimientoComponent,
    NecesidadesInversionComponent,
    DeudaEmprendimientoComponent,
    ApartadoAlertaEmprendimientoRequeridoComponent,
    ContenidoNuevaPersonaComponent,
    ContenidoEditarPersonaComponent,
    ModalModificarGrupoIntegranteComponent,
    ONGComponent,
    ActualizarDatosComponent,
    ActualizarCondicionesSolicitadasComponent
  ],
  exports: [
    FormularioRoutingModule,
    FormularioComponent,
    CursoComponent,
    ContenedorCuadrantesDirective,
    Linea2PieComponent,
    CondicionesSolicitadasComponent,
    DestinoFondosComponent,
    BusquedaPersonaComponent,
    SolicitanteComponent,
    IntegrantesPersonasComponent,
    IntegrantesGrillaComponent,
    GaranteComponent,
    BandejaFormularioComponent,
    EncabezadoComponent,
    GrupoFamiliarSolicitanteComponent,
    GrupoFamiliarGaranteComponent,
    CuadranteRequisitosComponent,
    PatrimonioSolicitanteComponent,
    DatosEmprendimientoComponent,
    OrganizacionIndividualComponent,
    MercadoYComercializacionComponent,
    PrecioVentaComponent,
    DescripcionProyectoComponent,
    IngresosYGastosActualesComponent,
    InversionRealizadaComponent,
    ResultadoEstimadoMensualComponent,
    ONGComponent,
    ActualizarDatosComponent,
  ],
  providers: [
    FormulariosService,
    CursosService,
    DestinoFondosService,
    LocalidadComboServicio,
    OrigenService,
    EstadoFormularioService,
    LineaService,
    PersonaService,
    SexoService,
    PaisService,
    ValidacionFormularioServicio,
    ActualizacionMasivaCuadrantesFormulariosService,
    MotivoRechazoService,
    EmprendimientoService,
    MotivoRechazoService,
  ],
  entryComponents: [
    CursoComponent,
    Linea2PieComponent,
    CondicionesSolicitadasComponent,
    DestinoFondosComponent,
    SolicitanteComponent,
    IntegrantesPersonasComponent,
    IntegrantesGrillaComponent,
    GaranteComponent,
    GrupoUnicoComponent,
    GrupoUnicoGaranteComponent,
    GrupoFamiliarSolicitanteComponent,
    GrupoFamiliarGaranteComponent,
    ModalMotivoBajaComponent,
    ModalSeleccionDetalleComponent,
    ModalVolverFormularioComponent,
    ModalMotivoRechazoComponent,
    ModalEditarNumeroCajaComponent,
    CuadranteRequisitosComponent,
    PatrimonioSolicitanteComponent,
    DatosEmprendimientoComponent,
    ModalDomicilioComponent,
    OrganizacionIndividualComponent,
    ModalIntegranteEmprendimientoComponent,
    DescripcionProyectoComponent,
    MercadoYComercializacionComponent,
    PrecioVentaComponent,
    DeudaEmprendimientoComponent,
    InversionRealizadaComponent,
    IngresosYGastosActualesComponent,
    NecesidadesInversionComponent,
    ResultadoEstimadoMensualComponent,
    ModalPlanPagosComponent,
    ModalVerMotivosRechazoComponent,
    ModalVerHistorialComponent,
    ResultadoEstimadoMensualComponent,
    ContenidoNuevaPersonaComponent,
    ContenidoEditarPersonaComponent,
    ModalModificarGrupoIntegranteComponent,
    ONGComponent
  ],
})

export class FormularioModule {
}
