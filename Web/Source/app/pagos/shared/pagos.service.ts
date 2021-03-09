import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Headers, Http, RequestOptions, Response } from '@angular/http';
import { BandejaPagosConsulta } from './modelo/bandeja-prestamo-consulta.model';
import { ErrorHandler } from '../../core/http/error-handler.service';
import { BandejaPagosResultado } from './modelo/bandeja-pagos-resultado.model';
import { Pagina, PaginaUtils } from '../../shared/paginacion/pagina-utils';
import { HttpUtils } from '../../shared/http-utils';
import { BandejaAsignarMontoDisponible } from './modelo/bandeja-asignar-monto-disponible.model';
import { MapUtils } from '../../shared/map-utils';
import { MontoDisponibleConsulta } from './modelo/monto-disponible-consulta.model';
import { TasasLoteResultado } from './modelo/tasas-lote-resultado.model';
import { ConfirmarLoteComando } from './modelo/confirmar-lote-comando.model';
import { FormularioPrestamo } from './modelo/formularios-prestamo.model';
import { BandejaLotesConsulta } from './modelo/bandeja-lotes-consulta.model';
import { BandejaLoteResultado } from './modelo/bandeja-lote-resultado.model';
import { DetalleLote } from '../monto-disponible/shared/modelo/detalle-lote.model';
import { HistorialLote } from './modelo/historial-lote.model';
import { PrestamosDetalleLoteConsulta } from './modelo/PrestamosDetalleLoteConsulta.model';
import { DesagruparLoteComando } from './modelo/desagrupar-lote-comando.model';
import { GenerarPlanPagosComando } from './modelo/generar-plan-pagos-comando.model';
import { ReporteResultado } from '../../shared/modelo/reporte-resultado.model';
import { BandejaArmarLoteSuafConsulta } from './modelo/bandeja-armar-lote-suaf-consulta.model';
import { BandejaArmarLoteSuafResultado } from './modelo/bandeja-armar-lote-suaf-resultado.model';
import { RegistrarLoteSuafComando } from './modelo/registrar-lote-suaf-comando.model';
import { FiltrosFormularioConsulta } from '../../seleccion-formularios/shared/modelos/filtros-formulario-consulta.model';
import { FormularioSeleccionado } from '../../seleccion-formularios/shared/modelos/formulario-seleccionado.model';
import { DetallesPlanPagosConsulta } from './modelo/detalles-plan-pagos-consulta.model';
import { LoteCombo } from './modelo/lote-combo.model';
import { BandejaSuafConsulta } from './modelo/bandeja-suaf-consulta.model';
import { BandejaSuafResultado } from './modelo/bandeja-suaf-resultado.model';
import { ImportacionSuafResultado } from './modelo/importacion-suaf-resultado.model';
import { CargaDevengadoComandoModel } from './modelo/carga-devengado-comando.model';
import { CrearNotaBancoConsulta } from './modelo/crear-nota-banco-comando.model';
import { ActualizarModalidadLoteComando } from './modelo/actualizar-modalidad-lote-comando.model';
import { PermiteLiberarLoteResultado } from './modelo/permite-liberar-lote-resultado.model';
import { FechasPago } from './modelo/fechas-pago.model';
import { ProvidenciaComando } from './modelo/providencia-comando.model';
import { ConsultaInformePagos } from '../../soporte/modelo/consulta-informe-pagos';
import { LineaAdendaResultado } from './modelo/linea-adenda-resultado.model';
import { GenerarAdendaComando } from './modelo/generar-adenda-comando.model';
import { BandejaCambioEstadoConsulta } from './modelo/bandeja-cambio-estado-consulta.model';
import { BandejaCambioEstadoResultado } from './modelo/bandeja-cambio-estado-resultado.model';
import { CargaDatosChequeComando } from './modelo/carga-datos-cheque-comando.model';
import { BandejaChequeConsulta } from './modelo/bandeja-cheque-consulta.model';
import { BandejaChequeResultado } from './modelo/bandeja-cheque-resultado.model';
import { AgregarPrestamoLoteComando } from './modelo/agregar-prestamo-lote-comando.model';
import { Convenio } from "../../shared/modelo/convenio-model";
import { TipoPagoCombo } from './modelo/tipo-pago-combo.model';
import {BandejaAdendaConsulta} from "./modelo/bandeja-adenda-consulta.model";
import {BandejaAdendaResultado} from "./modelo/bandeja-adenda-resultado.model";
import {DetallesAdenda} from "./modelo/detalles-adenda.model";
import {FormulariosSeleccionadosAdendaConsulta} from "./modelo/formularios-seleccionados-adenda-consulta.model";
import {FormulariosSeleccionadosAdendaResultado} from "./modelo/formularios-seleccionados-adenda-resultado.model";

@Injectable()
export class PagosService {
  public url: string = '/pagos';
  public static filtrosConsulta: BandejaPagosConsulta;
  public static filtrosConsultaLotes: BandejaLotesConsulta;
  public static filtrosConsultaSuaf: BandejaSuafConsulta;
  public static filtrosConsultaCheque: BandejaChequeConsulta;

  public static guardarFiltros(filtros: BandejaPagosConsulta): void {
    this.filtrosConsulta = filtros;
  }

  public static recuperarFiltros(): BandejaPagosConsulta {
    return this.filtrosConsulta;
  }

  public static guardarFiltrosLotes(filtros: BandejaLotesConsulta): void {
    this.filtrosConsultaLotes = filtros;
  }

  public static recuperarFiltrosLotes(): BandejaLotesConsulta {
    return this.filtrosConsultaLotes;
  }

  public static guardarFiltrosSuaf(filtros: BandejaSuafConsulta): void {
    this.filtrosConsultaSuaf = filtros;
  }

  public static recuperarFiltrosSuaf(): BandejaSuafConsulta {
    return this.filtrosConsultaSuaf;
  }

  public static guardarFiltrosCheque(filtros: BandejaChequeConsulta): void {
    this.filtrosConsultaCheque = filtros;
  }

  public static recuperarFiltrosCheque(): BandejaChequeConsulta {
    return this.filtrosConsultaCheque;
  }

  constructor(private http: Http,
              private errorHandler: ErrorHandler) {
  }

  public consultarBandeja(consulta: BandejaPagosConsulta): Observable<Pagina<BandejaPagosResultado>> {
    return this.http.post(`${this.url}/consultar/`, consulta)
      .map((res) => {
        return PaginaUtils.extraerPagina(BandejaPagosResultado, res);
      })
      .catch(this.errorHandler.handle);
  }

  public consultarBandejaCompleta(consulta: BandejaPagosConsulta): Observable<Pagina<BandejaPagosResultado>> {
    return this.http.post(`${this.url}/consultar-bandeja-completa/`, consulta)
      .map((res) => {
        return PaginaUtils.extraerPagina(BandejaPagosResultado, res);
      })
      .catch(this.errorHandler.handle);
  }

  public consultarMontosDisponibles(consulta: MontoDisponibleConsulta): Observable<BandejaAsignarMontoDisponible[]> {
    return this.http.get(`${this.url}/consultar-montos-disponible/`, {search: HttpUtils.insertarPrefijo(consulta)})
      .map(PagosService.extraerMontosDisponibles)
      .catch(this.errorHandler.handle);
  }

  private static extraerMontosDisponibles(response: Response): BandejaAsignarMontoDisponible[] {
    let resultado = response.json().resultado;
    if (resultado) {
      return (resultado || []).map((curso) => MapUtils.deserialize(BandejaAsignarMontoDisponible, curso));
    }
  }

  public obtenerTasas(): Observable<TasasLoteResultado> {
    return this.http.get(`${this.url}/obtener-iva-comision/`)
      .map(PagosService.extraerTasas)
      .catch(this.errorHandler.handle);
  }

  public obtenerTotalLote(idLoteSuaf: number): Observable<any> {
    return this.http.get(`${this.url}/obtener-total-lote?idLoteSuaf=${idLoteSuaf}`)
      .map((res: Response) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public habilitadoAdenda(idLoteSuaf: number): Observable<any> {
    return this.http.get(`${this.url}/habilitado-adenda?idLoteSuaf=${idLoteSuaf}`)
      .map((res: Response) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  private static extraerTasas(res: Response | any): TasasLoteResultado {
    let resultado = res.json().resultado;

    if (resultado) {
      return MapUtils.deserialize(TasasLoteResultado, resultado);
    }
  }

  public confirmarLote(comando: ConfirmarLoteComando): Observable<any> {
    return this.http
      .post(`${this.url}/confirmar-lote`, comando)
      .map((res) => {
        return res;
      })
      .catch(this.errorHandler.handle);
  }

  public agregarPrestamosLote(comando: AgregarPrestamoLoteComando): Observable<any> {
    return this.http
      .post(`${this.url}/agregar-prestamo-lote`, comando)
      .map((res) => {
        return res;
      })
      .catch(this.errorHandler.handle);
  }

  public confirmarLoteAdenda(comando: ConfirmarLoteComando): Observable<any> {
    return this.http
      .post(`${this.url}/confirmar-lote-adenda`, comando)
      .map((res) => {
        return res;
      })
      .catch(this.errorHandler.handle);
  }

  public consultarPrestamosParaAdenda(consulta: BandejaAdendaConsulta): Observable<Pagina<BandejaAdendaResultado>>{
    return this.http.post(`${this.url}/consultar-bandeja-adenda/`, consulta)
      .map((res) => {
        return PaginaUtils.extraerPagina(BandejaAdendaResultado, res);
      })
      .catch(this.errorHandler.handle);
  }

  public seleccionarTodosParaAdenda(consulta: BandejaAdendaConsulta): Observable<number>{
    return this.http.post(`${this.url}/seleccionar-todos-adenda/`, consulta)
      .map((res) => {
        if (res) {
          return res.json().resultado;
        }
      })
      .catch(this.errorHandler.handle);
  }

  public obtenerFormulariosSeleccionados(consulta: FormulariosSeleccionadosAdendaConsulta): Observable<Pagina<FormulariosSeleccionadosAdendaResultado>>{
    return this.http.post(`${this.url}/formularios-seleccionados-adenda/`, consulta)
      .map((res) => {
        return PaginaUtils.extraerPagina(FormulariosSeleccionadosAdendaResultado, res);
      })
      .catch(this.errorHandler.handle);
  }

  public modificarPrestamoAdenda(consulta: DetallesAdenda): Observable<number>{
    return this.http.post(`${this.url}/modificar-detalle-adenda/`, consulta)
      .map((res) => {
        if (res) {
          return res.json().resultado;
        }
      })
      .catch(this.errorHandler.handle);
  }

  public reporteDocumentacionPagos(consulta: {}): Observable<ReporteResultado> {
    return this.http.post(`${this.url}/reporte`, consulta)
      .map((res: Response) => {
        return res.json().resultado;
      }).catch(this.errorHandler.handle);
  }

  public reporteFormulariosNoImpresos(consulta: {}): Observable<ReporteResultado> {
    return this.http.post(`${this.url}/reporte-no-impreso`, consulta)
      .map((res: Response) => {
        return res.json().resultado;
      }).catch(this.errorHandler.handle);
  }

  public obtenerFormulariosPorPrestamo(idPrestamo: number): Observable<any> {
    return this.http.get(`${this.url}/formularios-por-prestamo?idPrestamo=${idPrestamo}`)
      .map((res) => MapUtils.extractModel(FormularioPrestamo, res))
      .catch(this.errorHandler.handle);
  }

  public consultarBandejaLotes(consulta: BandejaLotesConsulta): Observable<Pagina<BandejaLoteResultado>> {
    return this.http.get(`${this.url}/consultar-bandeja-lotes/`, {search: HttpUtils.insertarPrefijo(consulta)})
      .map((res) => {
        return PaginaUtils.extraerPagina(BandejaLoteResultado, res);
      })
      .catch(this.errorHandler.handle);
  }

  public consultarBandejaCheques(consulta: BandejaChequeConsulta): Observable<Pagina<BandejaChequeResultado>> {
    return this.http.get(`${this.url}/consultar-bandeja-cheque/`, {search: HttpUtils.insertarPrefijo(consulta)})
      .map((res) => {
        return PaginaUtils.extraerPagina(BandejaChequeResultado, res);
      })
      .catch(this.errorHandler.handle);
  }

  public liberarLote(idLote: number): Observable<any> {
    return this.http
      .post(`${this.url}/liberar-lote`, idLote)
      .map((res) => {
        return res;
      })
      .catch(this.errorHandler.handle);
  }

  public permiteLiberarLote(idLote: number): Observable<PermiteLiberarLoteResultado> {
    return this.http
      .get(`${this.url}/permite-liberar-lote?idLote=${idLote}`)
      .map((res) => {
        if (res) {
          return MapUtils.extractModel(PermiteLiberarLoteResultado, res);
        }
      })
      .catch(this.errorHandler.handle);
  }

  public obtenerCabeceraDetalleLote(idLote: number): Observable<DetalleLote> {
    return this.http
      .post(`${this.url}/obtener-cabecera-detalle-lote`, idLote)
      .map((res) => {
        if (res) {
          return MapUtils.extractModel(DetalleLote, res);
        }
      })
      .catch(this.errorHandler.handle);
  }

  public obtenerPrestamosDetalleLote(consulta: PrestamosDetalleLoteConsulta): Observable<Pagina<BandejaPagosResultado>> {
    return this.http
      .get(`${this.url}/obtener-prestamos-detalle-lote`, {search: HttpUtils.insertarPrefijo(consulta)})
      .map((res) => {
        if (res) {
          return PaginaUtils.extraerPagina(BandejaPagosResultado, res);
        }
      })
      .catch(this.errorHandler.handle);
  }

  public obtenerHistorialDetalleLote(idLote: number): Observable<any> {
    return this.http
      .post(`${this.url}/obtener-historial-detalle-lote`, idLote)
      .map((res) => {
        if (res) {
          return MapUtils.extractModel(HistorialLote, res);
        }
      })
      .catch(this.errorHandler.handle);
  }

  public desagruparLote(comando: DesagruparLoteComando): Observable<any> {
    return this.http
      .post(`${this.url}/desagrupar-lote`, comando)
      .map((res) => {
        return res;
      })
      .catch(this.errorHandler.handle);
  }

  public generarReporteExcelBanco(idLote: number): Observable<any> {
    return this.http.get(`${this.url}/obtener-excel-para-banco?idLote=${idLote}`)
      .map((res: Response) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public registrarChequeFormularios(idLote: number): Observable<any> {
    return this.http.get(`${this.url}/registrar-generacion-cheque?idLote=${idLote}`)
      .map((res: Response) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public validarProvidenciaLote(idLote: number): Observable<any> {
    return this.http.get(`${this.url}/validar-providencia-lote?idLote=${idLote}`)
      .map((res) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public generarArchivoTxt(idLote: number, idTipoPago: number): Observable<any> {
    return this.http.get(this.url + '/generar-archivo-txt'  + `/${idLote}` + `/${idTipoPago}`)
      .map((res: Response) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public obtenerNotaPago(consulta: CrearNotaBancoConsulta): Observable<any> {
    return this.http.get(`${this.url}/obtener-nota-pago`, {search: HttpUtils.insertarPrefijo(consulta)})
      .map((res: Response) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public actualizarPlanDePagos(comando: GenerarPlanPagosComando) {
    return this.http
      .post(`${this.url}/actualizar-plan-pagos`, comando)
      .map((res) => {
        return res;
      })
      .catch(this.errorHandler.handle);
  }

  public obtenerDetallesPlanDePagos(consulta: DetallesPlanPagosConsulta) {
    return this.http.post(`${this.url}/detalles-plan-pago`, consulta)
      .map((res: Response) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  // CONSULTA FORMULARIOS QUE PERTENECEN A UN LOTE
  public buscarFormulariosEnLoteFiltros(filtros: FiltrosFormularioConsulta): Observable<Pagina<FormularioSeleccionado>> {
    return this.http.post(this.url + '/buscar-formularios-en-lote', filtros)
      .map((res) => PaginaUtils.extraerPagina(FormularioSeleccionado, res))
      .catch(this.errorHandler.handle);
  }

  public buscarIdsFormulariosEnLoteFiltros(filtros: FiltrosFormularioConsulta): Observable<FormularioSeleccionado[]> {
    return this.http.post(this.url + '/buscar-ids-formularios-filtro-en-lote', filtros)
      .map((res) => MapUtils.extractModel(FormularioSeleccionado, res))
      .catch(this.errorHandler.handle);
  }

  public consultarBandejaArmarLoteSuaf(consulta: BandejaArmarLoteSuafConsulta): Observable<Pagina<BandejaArmarLoteSuafResultado>> {
    return this.http.get(`${this.url}/consultar-bandeja-formularios-suaf/`, {search: HttpUtils.insertarPrefijo(consulta)})
      .map((res) => {
        return PaginaUtils.extraerPagina(BandejaArmarLoteSuafResultado, res);
      })
      .catch(this.errorHandler.handle);
  }

  public consultarBandejaArmarLoteSuafSeleccionarTodos(consulta: BandejaArmarLoteSuafConsulta): Observable<Pagina<BandejaArmarLoteSuafResultado>> {
    return this.http.get(`${this.url}/consultar-bandeja-formularios-suaf-seleccionar-todos/`, {search: HttpUtils.insertarPrefijo(consulta)})
      .map((res) => {
        return PaginaUtils.extraerPagina(BandejaArmarLoteSuafResultado, res);
      })
      .catch(this.errorHandler.handle);
  }

  public registrarLoteSuaf(comando: RegistrarLoteSuafComando): Observable<number> {
    return this.http
      .post(`${this.url}/registrar-lote-suaf`, comando)
      .map((res) => {
        if (res) {
          return res.json().resultado;
        }
      })
      .catch(this.errorHandler.handle);
  }

  public generarReporteExcelSuaf(idLote: number): Observable<any> {
    return this.http.get(`${this.url}/obtener-excel-suaf?idLote=${idLote}`)
      .map((res: Response) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public generarExcelActivacionMasiva(idLote: number): Observable<any> {
    return this.http.get(`${this.url}/generar-excel-activacion-masiva?idLote=${idLote}`)
      .map((res: Response) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  private static extraerComboLote(res: Response): LoteCombo[] {
    let resultado = res.json().resultado;
    return (resultado || []).map((lote) => MapUtils.deserialize(LoteCombo, lote));
  }

  public obtenerComboLotes(tipoLote?: number): Observable<LoteCombo[]> {
    return this.http.get(`${this.url}/combo-lotes-pagos?tipoLote=${tipoLote}`)
      .map(PagosService.extraerComboLote)
      .catch(this.errorHandler.handle);
  }

  public consultarBandejaSuaf(consulta: BandejaSuafConsulta): Observable<Pagina<BandejaSuafResultado>> {
    return this.http.get(`${this.url}/consultar-bandeja-suaf/`, {search: HttpUtils.insertarPrefijo(consulta)})
      .map((res) => {
        return PaginaUtils.extraerPagina(BandejaSuafResultado, res);
      })
      .catch(this.errorHandler.handle);
  }

  public obtenerComboLotesSuaf(): Observable<LoteCombo[]> {
    return this.http.get(this.url + '/obtener-combo-lotes-suaf')
      .map(PagosService.extraerComboLote)
      .catch(this.errorHandler.handle);
  }

  public cargarDevengadoManual(comando: CargaDevengadoComandoModel): Observable<boolean> {
    return this.http
      .post(`${this.url}/cargar-devengado-manual`, comando)
      .map((res) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public borrarDevengado(comando: CargaDevengadoComandoModel): Observable<boolean> {
    return this.http
      .post(`${this.url}/borrar-devengado`, comando)
      .map((res) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public cargarDatosCheque(comando: CargaDatosChequeComando): Observable<boolean> {
    return this.http
      .post(`${this.url}/cargar-datos-cheque`, comando)
      .map((res) => {
        return res;
      })
      .catch(this.errorHandler.handle);
  }

  public obtenerModalidadesPago(): Observable<LoteCombo[]> {
    return this.http.get(this.url + '/modalidades-pago')
      .map((res) => MapUtils.extractModel(LoteCombo, res))
      .catch(this.errorHandler.handle);
  }

  public validarFormulariosLote(idLote: number): Observable<boolean> {
    return this.http.get(`${this.url}/validar-lote-pago?idLote=${idLote}`)
      .map((res) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public obtenerFechas(idLote: number): Observable<FechasPago> {
    return this.http.get(`${this.url}/obtener-fechas-pago?idLote=${idLote}`)
      .map((res) => MapUtils.extractModel(FechasPago, res))
      .catch(this.errorHandler.handle);
  }

  public obtenerElementosPago(): Observable<LoteCombo[]> {
    return this.http.get(this.url + '/elementos-pago')
      .map((res) => MapUtils.extractModel(LoteCombo, res))
      .catch(this.errorHandler.handle);
  }

  public obtenerConvenios(): Observable<Convenio[]> {
    return this.http.get(this.url + '/convenios')
      .map((res) => MapUtils.extractModel(LoteCombo, res))
      .catch(this.errorHandler.handle);
  }

  public registrarArchivoSuaf(archivo: any): Observable<ImportacionSuafResultado> {
    let headers = new Headers();
    let options = new RequestOptions({headers});
    let formData = HttpUtils.createFormData(archivo);

    return this.http
      .post('/pagos/importar-excel-suaf', formData, options)
      .map((res) => MapUtils.extractModel(ImportacionSuafResultado, res))
      .catch(this.errorHandler.handle);
  }

  public generarProvidenciaMasiva(comando: ProvidenciaComando): Observable<ReporteResultado> {
    return this.http
      .get('/providencia/reporte-providencia-masiva', {search: HttpUtils.insertarPrefijo(comando)})
      .map((res: Response) => {
        return res.json().resultado;
      }).catch(this.errorHandler.handle);
  }

  public actualizarModalidadPago(comando: ActualizarModalidadLoteComando): Observable<boolean> {
    return this.http
      .post(`${this.url}/actualizar-modalidad`, comando)
      .map((res) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public generarInformesBanco(comando: ConsultaInformePagos): Observable<any> {
    return this.http
      .post(`${this.url}/informes`, comando)
      .map((res: Response) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public imprimirPlanCuotas(consulta: DetallesPlanPagosConsulta) {
    return this.http.get(`${this.url}/imprimir-plan-cuotas`, {search: HttpUtils.insertarPrefijo(consulta)})
      .map((res: Response) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public obtenerLineasAdenda(nroDetalle: number): Observable<LineaAdendaResultado[]> {
    return this.http.get(`${this.url}/obtener-lineas-adenda?nroDetalle=${nroDetalle}`)
      .map((res) => MapUtils.extractModel(LineaAdendaResultado, res))
      .catch(this.errorHandler.handle);
  }

  public generarAdenda(comando: GenerarAdendaComando): Observable<boolean> {
    return this.http
      .post(`${this.url}/generar-adenda`, comando)
      .map((res) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public consultarBandejaCambioEstado(consulta: BandejaCambioEstadoConsulta): Observable<Pagina<BandejaCambioEstadoResultado>> {
    return this.http.get(`${this.url}/bandeja-cambio-estado/`, {search: HttpUtils.insertarPrefijo(consulta)})
      .map((res) => {
        return PaginaUtils.extraerPagina(BandejaCambioEstadoResultado, res);
      })
      .catch(this.errorHandler.handle);
  }

  public cambiarEstadoFormulario(idFormulario: number): Observable<boolean> {
    return this.http.get(`${this.url}/cambiar-estado-formulario?idFormulario=${idFormulario}`)
      .map((res) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }
  public consultarTotalizador(consulta: BandejaLotesConsulta) {
    return this.http.get(this.url + '/consultar-totalizador', {search: HttpUtils.insertarPrefijo(consulta)})
      .map((res) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }
  public consultarTotalizadorSuaf(consulta: BandejaSuafConsulta) {
    return this.http.get(this.url + '/consultar-totalizador-suaf', {search: HttpUtils.insertarPrefijo(consulta)})
      .map((res) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public consultarTotalizadorCheque(consulta: BandejaChequeConsulta) {
    return this.http.get(this.url + '/consultar-totalizador-cheque', {search: HttpUtils.insertarPrefijo(consulta)})
      .map((res) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public consultarTotalizadorCambioEstado(consulta: BandejaCambioEstadoConsulta) {
    return this.http.get(this.url + '/consultar-totalizador-estado', {search: HttpUtils.insertarPrefijo(consulta)})
      .map((res) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public consultarTipoPago() {
    return this.http.get(`${this.url}/obtener-combo-tipo-pago`)
      .map((res) => MapUtils.extractModel(TipoPagoCombo, res))
      .catch(this.errorHandler.handle);
  }

  public validarEstadoFormulario (idLote: number): Observable<boolean>  {
    return this.http.get(`${this.url}/validar-estados-formularios?idLote=${idLote}`)
      .map((res) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }
}
