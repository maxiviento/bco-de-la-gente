import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Formulario } from './modelo/formulario.model';
import { Observable } from 'rxjs';
import { Http, Response } from '@angular/http';
import { ErrorHandler } from '../../core/http/error-handler.service';
import { MapUtils } from '../../shared/map-utils';
import { BandejaFormularioConsulta } from './modelo/bandeja-formulario-consulta.model';
import { BandejaFormularioResultado } from './modelo/bandeja-formulario-resultado.model';
import { Pagina, PaginaUtils } from '../../shared/paginacion/pagina-utils';
import { HttpUtils } from '../../shared/http-utils';
import { Departamento } from './modelo/departamento.model';
import { DatosContacto } from '../../shared/modelo/datos-contacto.model';
import { ActualizarDatosDeContactoComando } from './modelo/actualizar-datos-contacto-comando.model';
import { RechazarFormularioComando } from './modelo/rechazar-formulario-comando.model';
import { Persona } from '../../shared/modelo/persona.model';
import { DetalleLineaFormulario } from './modelo/detalle-linea-formulario.model';
import { SolicitudCurso } from './modelo/solicitud-curso.model';
import { ObtenerDatosContactoConsulta } from '../../shared/modelo/consultas/obtener-datos-contacto-consulta.model';
import { OpcionDestinoFondos } from './modelo/opcion-destino-fondos';
import { CondicionesSolicitadas } from './modelo/condiciones-solicitadas.model';
import { Emprendimiento } from './modelo/emprendimiento.model';
import { Subject } from 'rxjs/Subject';
import { PatrimonioSolicitante } from './modelo/patrimonio-solicitante.model';
import { FiltrosFormularioConsulta } from '../../seleccion-formularios/shared/modelos/filtros-formulario-consulta.model';
import { FormularioSeleccionado } from '../../seleccion-formularios/shared/modelos/formulario-seleccionado.model';
import { ReporteResultado } from '../../shared/modelo/reporte-resultado.model';
import { ItemInversion } from './modelo/item-inversion.model';
import { TipoDeudaEmprendimiento } from './modelo/tipo-deuda.emprendimiento';
import { DeudaEmprendimiento } from './modelo/deuda.emprendimiento';
import { IngresoGrupo } from './modelo/ingreso-grupo.model';
import { IngresoGrupoComando } from './modelo/ingreso-grupo-comando.model';
import { InversionRealizada } from './modelo/inversion-realizada.model';
import { NecesidadInversion } from './modelo/necesidad-inversion.model';
import { ActualizarPrecioVentaComando } from './modelo/actualizar-precio-venta-comando.model';
import { FuenteFinanciamiento } from './modelo/fuente.financiamiento';
import { Costo } from './modelo/costo.model';
import { GastoGrupo } from './modelo/gastos-grupo.model';
import { Integrante } from "../../shared/modelo/integrante.model";
import { FormularioidDocumento } from "./modelo/formularioid-documento.model";
import { FormularioIdAgrupamientoId } from "./modelo/formularioid-agrupamientoid.model";
import { OngComboResultado } from './modelo/ong.model';
import { OngFormulario } from './modelo/ong-formulario.model';
import { BandejaCargaNumeroControlInterno } from '../../prestamos-checklists/shared/modelos/bandeja-carga-numero-control-interno.model';
import { CargarNumeroControlInternoComando } from '../../prestamos-checklists/shared/modelos/cargar-numero-control-interno-comando.model';
import { FormularioFechaPago } from './modelo/formulario-fecha-pago.model';
import { Reprogramacion } from './modelo/procesos-varios.model';
import { GrupoDomicilioIntegranteResultado } from '../../prestamos-checklists/shared/modelos/grupo-domicilio-integrante-resultado.model';
import { DomicilioIntegrante } from '../../prestamos-checklists/shared/modelos/domicilio-integrante.model';
import { MotivoRechazoFormulario } from "./modelo/motivo-rechazo-formulario-model";
import { IdsDetalleLineas } from "./modelo/ids-detalle-lineas-.model";
import { CambiarGaranteComando } from './modelo/cambiar-garante-comando.model';
import { ConsultarGrupoFamiliarAgrupados } from "./modelo/consultar-grupo-familiar-agrupados.model";

@Injectable()
export class FormulariosService {
  public static filtrosConsulta: BandejaFormularioConsulta;

  public static guardarFiltros(filtros: BandejaFormularioConsulta): void {
    this.filtrosConsulta = filtros;
  }

  public consultarTotalizador(consulta: BandejaFormularioConsulta) {
    return this.http.post(`${this.url}/consultar-totalizador/`, consulta)
      .map((res) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public static recuperarFiltros(): BandejaFormularioConsulta {
    return this.filtrosConsulta;
  }

  private static extraerDatosContacto(res: Response | any): DatosContacto {
    let resultado = res.json().resultado;

    if (resultado) {
      return MapUtils.deserialize(DatosContacto, resultado);
    }
  }

  private static extraerFormularioCargado(res: Response): Formulario {
    let resultado = res.json().resultado;

    if (resultado) {
      return MapUtils.deserialize(Formulario, resultado);
    }
  }

  private static extraerDetalleLineaFormulario(res: Response): DetalleLineaFormulario[] {
    let resultado = res.json().resultado;
    return (resultado || []).map((detalles) => MapUtils.deserialize(DetalleLineaFormulario, detalles));
  }

  public url: string = '/Formularios';
  public formulario: Formulario;
  public coordinador: BehaviorSubject<Formulario>;

  constructor(private http: Http,
              private errorHandler: ErrorHandler) {
  }

  public inicializarFormularioNuevo(detalleLinea: DetalleLineaFormulario, idOrigen: number): Formulario {
    this.formulario = new Formulario(null, detalleLinea, idOrigen);
    this.formulario.integrantes = [];
    this.coordinador = new BehaviorSubject<Formulario>(this.formulario);
    return this.formulario;
  }

  public inicializarFormularioCargado(formulario: Formulario): Formulario {
    this.formulario = formulario;
    this.coordinador = new BehaviorSubject<Formulario>(this.formulario);
    return this.formulario;
  }

  public obtenerDetallesLinea() {
    return this.http.get('/LineasPrestamo/detalles-seleccion')
      .map(FormulariosService.extraerDetalleLineaFormulario)
      .catch(this.errorHandler.handle);
  }

  public consultarFormulario(idFormulario: number): Observable<Formulario> {
    return this.http
      .get(`${this.url}/${idFormulario}`)
      .map(FormulariosService.extraerFormularioCargado)
      .catch(this.errorHandler.handle);
  }

  public RegistrarFormulario(formulario: Formulario): Observable<FormularioIdAgrupamientoId> {
    return this.http
      .post(this.url, formulario)
      .map((res) => MapUtils.extractModel(FormularioIdAgrupamientoId, res))
      .catch(this.errorHandler.handle);
  }

  public RegistrarFormularios(formulario: Formulario): Observable<FormularioidDocumento[]> {
    return this.http
      .post(`${this.url}/registrar-formularios`, formulario)
      .map((res) => MapUtils.extractModel(FormularioidDocumento, res))
      .catch(this.errorHandler.handle);
  }

  public ValidarEstadosFormulariosAgrupados(idAgrupamiento: number): Observable<boolean> {
    return this.http.get(`${this.url}/validar-estados-formularios/${idAgrupamiento}`)
      .map((response) => response.json().resultado)
      .catch(this.errorHandler.handle);
  }

  public EnviarFormulario(formulario: Formulario): Observable<number> {
    return this.http
      .post(`${this.url}/enviar`, formulario)
      .map((response) => response.json().resultado)
      .catch(this.errorHandler.handle);
  }

  public actualizarCursos(idFormulario: number, solicitudesCurso: SolicitudCurso[]): Observable<any> {
    return this.http
      .post(`${this.url}/actualizar-cursos/${idFormulario}`, solicitudesCurso)
      .catch(this.errorHandler.handle);
  }

  public actualizarCursosAsociativas(idAgrupamiento: number, solicitudesCurso: SolicitudCurso[]): Observable<any> {
    return this.http
      .post(`${this.url}/actualizar-cursos-asociativas/${idAgrupamiento}`, solicitudesCurso)
      .catch(this.errorHandler.handle);
  }

  public IniciarFormulario(formulario: Formulario): Observable<number> {
    return this.http
      .post(`${this.url}/iniciar`, formulario)
      .map((response) => response.json().resultado)
      .catch(this.errorHandler.handle);
  }

  public consultarFormularios(consultaFormulario: BandejaFormularioConsulta): Observable<Pagina<BandejaFormularioResultado>> {
    return this.http
      .post(`${this.url}/consultar/`, consultaFormulario)
      .map((res) => {
        return PaginaUtils.extraerPagina(BandejaFormularioResultado, res);
      })
      .catch(this.errorHandler.handle);
  }

  public existeFormularioEnCursoParaPersona(persona: Persona): Observable<number> {
    let consulta = this.armarConsultaDatosContacto(persona);
    return this.http
      .get(this.url + '/existeFormularioEnCursoParaSolicitante', {search: HttpUtils.insertarPrefijo(consulta)})
      .map((response) =>
        response.json().resultado)
      .catch(this.errorHandler.handle);
  }

  public existeFormularioEnCursoParaPersonaReactivacion(persona: Persona): Observable<number> {
    let consulta = this.armarConsultaDatosContacto(persona);
    return this.http
      .get(this.url + '/existeFormularioEnCursoParaSolicitanteReactivacion', {search: HttpUtils.insertarPrefijo(consulta)})
      .map((response) =>
        response.json().resultado)
      .catch(this.errorHandler.handle);
  }

  public existeDeudaHistorica(persona: Persona): Observable<boolean> {
    let consulta = this.armarConsultaDatosContacto(persona);
    return this.http
      .get(this.url + '/existeDeudaHistorica', {search: HttpUtils.insertarPrefijo(consulta)})
      .map((response) =>
        response.json().resultado)
      .catch(this.errorHandler.handle);
  }

  public existeGrupoSolicitante(idFormulario: number): Observable<boolean> {
    return this.http
      .get(`${this.url}/existeGrupoSolicitante?idFormulario=${idFormulario}`)
      .map((response) =>
        response.json().resultado)
      .catch(this.errorHandler.handle);
  }

  public existeFormularioEnCursoParaGrupo(persona: Persona): Observable<any[]> {
    let consulta = this.armarConsultaDatosContacto(persona);
    return this.http
      .get(this.url + '/existeFormularioEnCursoParaGrupoFamiliar', {search: HttpUtils.insertarPrefijo(consulta)})
      .map((response) =>
        response.json().resultado
      )
      .catch(this.errorHandler.handle);
  }

  public miembroGrupoPerteneceAgrupamiento(persona: Persona, idAgrupamiento: number): Observable<any[]> {
    let consulta = this.armarConsultaFamiliaAgrupada(persona, idAgrupamiento);
    return this.http
      .get(this.url + '/familia-pertenece-agrupamiento', {search: HttpUtils.insertarPrefijo(consulta)})
      .map((response) =>
        response.json().resultado
      )
      .catch(this.errorHandler.handle);
  }

  public consultarDepartamentos() {
    return this.http
      .get('/Departamentos')
      .map((response) => {
        let resultado = response.json().resultado;
        return (resultado || []).map((departamento) => MapUtils.deserialize(Departamento, departamento));
      });
  }

  public rechazarFormulario(comando: RechazarFormularioComando): Observable<any> {
    return this.http
      .post(`${this.url}/rechazar-formulario`, comando)
      .map((res) => {
        return res;
      }).catch(this.errorHandler.handle);
  }

  public consultarBandejaCargaNumeroControlInterno(idFormularioLinea: number): Observable<BandejaCargaNumeroControlInterno> {
    return this.http.get(`${this.url}/consulta-bandeja-carga-numero-control-interno?idFormularioLinea=${idFormularioLinea}`)
      .map((res) => MapUtils.extractModel(BandejaCargaNumeroControlInterno, res))
      .catch(this.errorHandler.handle);
  }

  public consultarSituacionDomicilioIntegrante(idFormularioLinea: number): Observable<GrupoDomicilioIntegranteResultado[]> {
    return this.http.get(`${this.url}/consulta-situacion-domicilio-integrante?idFormularioLinea=${idFormularioLinea}`)
      .map((res) => MapUtils.extractModel(GrupoDomicilioIntegranteResultado, res))
      .catch(this.errorHandler.handle);
  }

  public obtenerDomicilioIntegrante(idFormularioLinea: number): Observable<DomicilioIntegrante> {
    return this.http.get(`${this.url}/consulta-datos-domicilio-integrante?idFormularioLinea=${idFormularioLinea}`)
      .map((res) => MapUtils.extractModel(DomicilioIntegrante, res))
      .catch(this.errorHandler.handle);
  }

  public rechazarFormularioConPrestamo(comando: RechazarFormularioComando): Observable<any> {
    return this.http
      .post(`${this.url}/rechazar-formulario-con-prestamo`, comando)
      .map((res) => {
        return res;
      }).catch(this.errorHandler.handle);
  }

  public darDeBajaFormulario(comando: RechazarFormularioComando): Observable<any> {
    return this.http
      .post(`${this.url}/baja-formulario`, comando)
      .map((res) => {
        return res;
      })
      .catch(this.errorHandler.handle);
  }

  public guardarNumeroControlInterno(comando: CargarNumeroControlInternoComando): Observable<object> {
    return this.http.post(this.url + '/cargar-numero-control-interno', comando)
      .map((res) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public rechazarGrupoFormularios(comando: RechazarFormularioComando): Observable<any> {
    return this.http
      .post(`${this.url}/rechazar-grupo-formularios`, comando)
      .map((res) => {
        return res;
      })
      .catch(this.errorHandler.handle);
  }

  public obtenerDatosDeContacto(persona: Persona): Observable<DatosContacto> {
    let consulta = this.armarConsultaDatosContacto(persona);
    return this.http.get(this.url + '/consulta-datos-contacto', {search: HttpUtils.insertarPrefijo(consulta)})
      .map(FormulariosService.extraerDatosContacto)
      .catch(this.errorHandler.handle);
  }

  public actualizarDatosDeContacto(comando: ActualizarDatosDeContactoComando): Observable<any> {
    return this.http
      .post(`${this.url}/actualizar-datos-contacto`, comando)
      .map((response) => response.json().resultado)
      .catch(this.errorHandler.handle);
  }

  private armarConsultaDatosContacto(persona: Persona): ObtenerDatosContactoConsulta {
    return new ObtenerDatosContactoConsulta(
      persona.sexoId,
      persona.nroDocumento,
      persona.codigoPais,
      persona.idNumero.toString());
  }

  private armarConsultaFamiliaAgrupada(persona: Persona, idAgrupamiento: number): ConsultarGrupoFamiliarAgrupados {
    return new ConsultarGrupoFamiliarAgrupados(
      persona.sexoId,
      persona.nroDocumento,
      persona.codigoPais,
      persona.idNumero.toString(),
      idAgrupamiento);
  }

  public actualizarDestinosFondos(idFormulario: number, destinosFondos: OpcionDestinoFondos): Observable<any> {
    return this.http
      .post(`${this.url}/actualizar-destinos-fondos/${idFormulario}`, destinosFondos)
      .catch(this.errorHandler.handle);
  }

  public actualizarDestinosFondosAsociativas(idAgrupamiento: number, destinosFondos: OpcionDestinoFondos): Observable<any> {
    return this.http
      .post(`${this.url}/actualizar-destinos-fondos-asociativas/${idAgrupamiento}`, destinosFondos)
      .catch(this.errorHandler.handle);
  }

  public actualizarCondicionesSolicitadas(idFormulario: number, condicionesSolicitadas: CondicionesSolicitadas): Observable<any> {
    return this.http
      .post(`${this.url}/actualizar-condiciones-solicitadas/${idFormulario}`, condicionesSolicitadas)
      .catch(this.errorHandler.handle);
  }
  public modificarCondicionesSolicitadas(idFormulario: number, condicionesSolicitadas: CondicionesSolicitadas): Observable<string> {
    return this.http
      .post(`${this.url}/modificar-condiciones-solicitadas/${idFormulario}`, condicionesSolicitadas)
      .map((res) => res.json().resultado)
      .catch(this.errorHandler.handle);
  }

  public actualizarCondicionesSolicitadasAsociativas(idAgrupamiento: number, condicionesSolicitadas: CondicionesSolicitadas): Observable<any> {
    return this.http
      .post(`${this.url}/actualizar-condiciones-solicitadas-asociativas/${idAgrupamiento}`, condicionesSolicitadas)
      .catch(this.errorHandler.handle);
  }

  public actualizarGarantes(idFormulario: number, garantes: Persona[]): Observable<any> {
    return this.http
      .post(`${this.url}/actualizar-garantes/${idFormulario}`, garantes)
      .catch(this.errorHandler.handle);
  }

  public actualizarEmprendimiento(idAgrupamiento: number, emprendimiento: Emprendimiento): Observable<number> {
    return this.http
      .post(`${this.url}/actualizar-emprendimiento/${idAgrupamiento}`, emprendimiento)
      .map((res) => res.json().resultado)
      .catch(this.errorHandler.handle);
  }

  public obtenerLogo(idDetalleLinea: number) {
    return this.http
      .get(`/lineasprestamo/logo/${idDetalleLinea}`)
      .map((response) => response.json().resultado)
      .catch(this.errorHandler.handle);
  }

  public generarReporteFormulario(id: number): Observable<string> {
    let params = new URLSearchParams();
    params.set('id', id.toString());
    return this.http.get(this.url + `/reporte?id=${id}`)
      .map((res: Response) => {
        return res.json().resultado;
      }).catch(this.errorHandler.handle);
  }

  public generarReporteFormularioLinea(ids: IdsDetalleLineas): Observable<string> {
    return this.http.get(this.url + `/reporte-linea`, {search: HttpUtils.insertarPrefijo(ids)})
      .map((res: Response) => {
        return res.json().resultado;
      }).catch(this.errorHandler.handle);
  }

  private personaEncontrada = new Subject<Persona>();
  public personaEncontrada$ = this.personaEncontrada.asObservable();

  public almacenarPersona(persona: Persona): void {
    this.personaEncontrada.next(persona);
  }

  public agregarSucursalFormularios(actSucursalComando: {}): Observable<any> {
    return this.http.post(this.url + '/actualiza-sucursal', actSucursalComando)
      .map((res) => MapUtils.extractModel(Formulario, res))
      .catch(this.errorHandler.handle);
  }

  public consultaDomicilio(persona: Persona): Observable<any> {
    return this.http.post('/domicilios/consulta-domicilio-generado', persona)
      .map((res) => MapUtils.extractModel(Formulario, res))
      .catch(this.errorHandler.handle);
  }

  public buscarFormulariosFiltros(filtros: FiltrosFormularioConsulta, uri: string): Observable<Pagina<FormularioSeleccionado>> {
    return this.http.post(this.url + '/' + uri, filtros)
      .map((res) => PaginaUtils.extraerPagina(FormularioSeleccionado, res))
      .catch(this.errorHandler.handle);
  }
  public consultarTotalizadorSucursalBancaria(consulta: FiltrosFormularioConsulta) {
    return this.http.get(this.url + '/consultar-totalizador-sucursal', {search: HttpUtils.insertarPrefijo(consulta)})
      .map((res) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public consultarTotalizadorDocumentacion(consulta: FiltrosFormularioConsulta) {
    return this.http.get(this.url + '/consultar-totalizador-documentacion', {search: HttpUtils.insertarPrefijo(consulta)})
      .map((res) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public buscarIdsFormulariosFiltros(filtros: FiltrosFormularioConsulta, uri: string): Observable<FormularioSeleccionado []> {
    return this.http.post(this.url + '/' + uri, filtros)
      .map((res) => MapUtils.extractModel(FormularioSeleccionado, res))
      .catch(this.errorHandler.handle);
  }

  public actualizarPatrimonioSolicitante(idFormulario: number, patrimonioSolicitante: PatrimonioSolicitante): Observable<any> {
    return this.http
      .post(`${this.url}/actualizar-patrimonio-solicitante/${idFormulario}`, patrimonioSolicitante)
      .catch(this.errorHandler.handle);
  }

  public actualizarPatrimonioSolicitanteAsociativas(idAgrupamiento: number, patrimonioSolicitante: PatrimonioSolicitante): Observable<any> {
    return this.http
      .post(`${this.url}/actualizar-patrimonio-solicitante-asociativas/${idAgrupamiento}`, patrimonioSolicitante)
      .catch(this.errorHandler.handle);
  }

  public obtenerPatrimonioSolicitante(idFormulario: number): Observable<PatrimonioSolicitante> {
    return this.http
      .get(`${this.url}/obtener-patrimonio-solicitante/${idFormulario}`)
      .map((res) => MapUtils.extractModel(PatrimonioSolicitante, res))
      .catch(this.errorHandler.handle);
  }

  public reporteDeudaGrupoConviviente(filtros: FiltrosFormularioConsulta): Observable<ReporteResultado> {
    return this.http.get(`${this.url}/reporte-deuda-grupo-conviviente`, {search: HttpUtils.insertarPrefijo(filtros)})
      .map((res: Response) => {
        return res.json().resultado;
      }).catch(this.errorHandler.handle);
  }

  public existeGrupoPersona(persona: Persona): Observable<boolean> {
    let filtros = this.armarConsultaDatosContacto(persona);
    return this.http.get(this.url + '/existeGrupoPersona', {search: HttpUtils.insertarPrefijo(filtros)})
      .map((response) =>
        response.json().resultado
      )
      .catch(this.errorHandler.handle);
  }

  public obtenerDatosEmprendimiento(idFormulario: number): Observable<Emprendimiento> {
    return this.http
      .get(`${this.url}/datos-emprendimiento/${idFormulario}`)
      .map((res) => MapUtils.extractModel(Emprendimiento, res))
      .catch(this.errorHandler.handle);
  }

  public actualizarOrganizacionEmprendimiento(idFormulario: number, comando: any): Observable<any> {
    return this.http
      .post(`${this.url}/actualizar-organizacion-emprendimiento/${idFormulario}`, comando)
      .catch(this.errorHandler.handle);
  }

  public obtenerItemsInversion(): Observable<ItemInversion[]> {
    return this.http
      .get(`${this.url}/obtener-items-inversion`)
      .map((res) => this.extraerItemsInversion(res))
      .catch(this.errorHandler.handle);
  }

  private extraerItemsInversion(res: Response): ItemInversion[] {
    let resultado = res.json().resultado;
    return (resultado || []).map((resultados) => MapUtils.deserialize(ItemInversion, resultados));
  }

  public obtenerFuentesFinanciamiento(): Observable<FuenteFinanciamiento[]> {
    return this.http
      .get(`${this.url}/obtener-fuentes-financiamiento`)
      .map((res) => this.extraerFuentesFinanciamiento(res))
      .catch(this.errorHandler.handle);
  }

  public obtenerMotivosRechazoFormulario(idFormulario: number): Observable<MotivoRechazoFormulario[]> {
    return this.http.get(`${this.url}/obtener-motivos-rechazo-formulario/${idFormulario}`)
      .map((response) => response.json().resultado)
      .catch(this.errorHandler.handle);
  }

  private extraerFuentesFinanciamiento(res: Response): ItemInversion[] {
    let resultado = res.json().resultado;
    return (resultado || []).map((resultados) => MapUtils.deserialize(ItemInversion, resultados));
  }

  public obtenerTiposDeuda(): Observable<TipoDeudaEmprendimiento[]> {
    return this.http
      .get(`${this.url}/obtener-tipos-deuda`)
      .map((res) => this.extraerTiposDeuda(res))
      .catch(this.errorHandler.handle);
  }

  private extraerTiposDeuda(res: Response): TipoDeudaEmprendimiento[] {
    let resultado = res.json().resultado;
    return (resultado || []).map((resultados) => MapUtils.deserialize(TipoDeudaEmprendimiento, resultados));
  }

  public actualizarDeudaEmprendimiento(idFormulario: number, deudasEmprendimiento: DeudaEmprendimiento[]): Observable<any> {
    return this.http
      .post(`${this.url}/actualizar-deuda-emprendimiento/${idFormulario}`, deudasEmprendimiento)
      .catch(this.errorHandler.handle);
  }

  public actualizarInversionesRealizadas(idFormulario: number, inversionesRealizadas: InversionRealizada[]): Observable<any> {
    return this.http
      .post(`${this.url}/actualizar-inversiones-realizadas/${idFormulario}`, inversionesRealizadas)
      .map((res) => MapUtils.extractModel(InversionRealizada, res))
      .catch(this.errorHandler.handle);
  }

  public actualizarNecesidadInversion(idFormulario: number, necesidadInversion: NecesidadInversion): Observable<any> {
    return this.http
      .post(`${this.url}/actualizar-necesidades-inversion/${idFormulario}`, necesidadInversion)
      .catch(this.errorHandler.handle);
  }

  public eliminarDetallesInversion(detallesParaEliminar: number[]): Observable<any> {
    return this.http
      .post(`${this.url}/eliminar-detalles-inversion`, detallesParaEliminar)
      .catch(this.errorHandler.handle);
  }

  public obtenerIngresosGrupoFamiliar(): Observable<IngresoGrupo[]> {
    return this.http
      .get(`${this.url}/ingresos-grupo`)
      .map((res) => MapUtils.extractModel(IngresoGrupo, res))
      .catch(this.errorHandler.handle);
  }

  public obtenerIngresoTotalGrupoFamiliar(idGrupo: number): Observable<number> {
    return this.http
      .get(`${this.url}/ingreso-total-grupo-familiar/${idGrupo}`)
      .map((response) => response.json().resultado)
      .catch(this.errorHandler.handle);
  }

  public obtenerGastosGrupoFamiliar(idGrupo: number): Observable<GastoGrupo[]> {
    return this.http
      .get(`${this.url}/gastos-grupo/${idGrupo}`)
      .map((res) => MapUtils.extractModel(GastoGrupo, res))
      .catch(this.errorHandler.handle);
  }

  public obtenerIngresosGrupoFamiliarFormulario(idFormulario: number): Observable<IngresoGrupo[]> {
    return this.http
      .get(`${this.url}/ingresos-grupo/${idFormulario}`)
      .map((res) => MapUtils.extractModel(IngresoGrupo, res))
      .catch(this.errorHandler.handle);
  }

  public actualizarIngresosGrupoFamiliar(comando: IngresoGrupoComando): Observable<any> {
    return this.http
      .post(`${this.url}/actualizar-ingresos-grupo`, comando)
      .catch(this.errorHandler.handle);
  }

  public actualizarCuadrantePrecioVenta(idFormulario: number, comando: ActualizarPrecioVentaComando): Observable<number> {
    return this.http
      .post(`${this.url}/actualizar-cuadrante-precio-venta/${idFormulario}`, comando)
      .map((response) => response.json().resultado)
      .catch(this.errorHandler.handle);
  }

  public obtenerCostosActualizados(idFormulario: number): Observable<Costo[]> {
    return this.http
      .get(`${this.url}/precio-venta/costos/${idFormulario}`)
      .map((res) => MapUtils.extractModel(Costo, res))
      .catch(this.errorHandler.handle);
  }

  public obtenerIntegrantes(idAgrupamiento: number): Observable<Integrante[]> {
    return this.http
      .get(`${this.url}/obtener-formularios-agrupados/${idAgrupamiento}`)
      .map((res) => MapUtils.extractModel(Integrante, res))
      .catch(this.errorHandler.handle);
  }

  public obtenerAgrupamientoFormulario(idFormulario: number) {
    return this.http.get(`${this.url}/obtener-agrupamiento/${idFormulario}`)
      .map((res) => res.json().resultado)
      .catch(this.errorHandler.handle);
  }

  public obtenerEstadoFormulario(idEstado: number): Observable<string> {
    return this.http.get(`${this.url}/obtener-estado-formulario/${idEstado}`)
      .map((res) => res.json().resultado)
      .catch(this.errorHandler.handle);
  }

  public obtenerOngs(idLinea: number): Observable<OngComboResultado[]> {
    return this.http.get(`${this.url}/obtener-ongs/${idLinea}`)
      .map((res) => res.json().resultado)
      .catch(this.errorHandler.handle);
  }

  public obtenerNumeroGrupo(comando: OngFormulario): Observable<number> {
    return this.http
      .post(`${this.url}/obtener-numero-grupo`, comando)
      .map((response) => response.json().resultado)
      .catch(this.errorHandler.handle);
  }

  public registrarOngParaFormularios(idAgrupamiento: number, ong: OngFormulario): Observable<any> {
    return this.http
      .post(`${this.url}/registrar-ong-para-formulario/${idAgrupamiento}`, ong)
      .catch(this.errorHandler.handle);
  }

  public obtenerFormularioFechaPago(idFormulario: number): Observable<any> {
    return this.http
      .get(`${this.url}/obtener-formulario-fecha-pago/${idFormulario}`)
      .map((res) => MapUtils.extractModel(FormularioFechaPago, res))
      .catch(this.errorHandler.handle);
  }

  public obtenerCondicionesSolicitadas(idFormulario: number): Observable<Formulario> {
    return this.http
      .get(`${this.url}/obtener-condiciones-solicitadas/${idFormulario}`)
      .map(FormulariosService.extraerFormularioCargado)
      .catch(this.errorHandler.handle);
  }

  public obtenerTiempoReprogramacion() {
    return this.http.get(`${this.url}/obtener-tiempo-reprogramacion`)
      .map((res) => res.json().resultado)
      .catch(this.errorHandler.handle);
  }

  public obtenerHistorialReprogramacion(idFormulario: number): Observable<Reprogramacion[]> {
    return this.http
      .get(`${this.url}/obtener-historial-reprogramacion/${idFormulario}`)
      .map((res) => MapUtils.extractModel(Reprogramacion, res))
      .catch(this.errorHandler.handle);
  }

  public cambiarGaranteFormulario(comando: CambiarGaranteComando): Observable<boolean> {
    return this.http
      .post(`${this.url}/cambiar-garante-formulario`, comando)
      .map((response) =>
        response.json().resultado)
      .catch(this.errorHandler.handle);
  }

  public generarReporteBandejaExcel(consultaFormulario: BandejaFormularioConsulta): Observable<any> {
    return this.http.post(`${this.url}/obtener-reporte-excel-bandeja-formularios`, consultaFormulario)
      .map((res) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public generarReporteBandejaPDF(consultaFormulario: BandejaFormularioConsulta): Observable<ReporteResultado> {
    return this.http
      .post(this.url + '/obtener-reporte-pdf-bandeja-formularios', consultaFormulario)
      .map((res) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public obtenerNroSuac(idFormulario: number) {
    return this.http.get(`${this.url}/obtener-nro-suac/${idFormulario}`)
    .map((res) => {return res.json().resultado;})
    .catch(this.errorHandler.handle);
  }
}
