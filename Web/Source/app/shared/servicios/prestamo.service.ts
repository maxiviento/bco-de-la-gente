import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs';
import { ErrorHandler } from '../../core/http/error-handler.service';
import { Prestamo } from '../../prestamos-checklists/shared/modelos/prestamo.model';
import { IntegrantePrestamo } from '../modelo/integrante-prestamo.model';
import { MapUtils } from '../map-utils';
import { HttpUtils } from '../http-utils';
import { ConsultaBandejaPrestamos } from '../../prestamos-checklists/shared/modelos/consulta-bandeja-prestamos.model';
import { BandejaConformarPrestamoConsulta } from '../../prestamos-checklists/shared/modelos/consulta-bandeja-conformar-prestamo.model';
import { Pagina, PaginaUtils } from '../paginacion/pagina-utils';
import { BandejaPrestamoResultado } from '../../prestamos-checklists/shared/modelos/bandeja-prestamo-resultado.model';
import { RequisitoPrestamo } from '../../prestamos-checklists/shared/modelos/requisito-prestamo';
import { SeguimientoPrestamo } from '../../prestamos-checklists/shared/modelos/seguimiento-prestamo';
import { ConsultaSeguimientosPrestamo } from '../../prestamos-checklists/shared/modelos/consulta-seguimientos';
import { RechazarPrestamoComando } from '../../prestamos-checklists/shared/modelos/rechazar-prestamo-comando.model';
import { EstadoPrestamo } from '../../prestamos-checklists/shared/modelos/estado-prestamo.model';
import { EncabezadoPrestamo } from '../../prestamos-checklists/shared/modelos/encabezado-prestamo.model';
import { RegistrarPrestamoResultado } from '../../prestamos-checklists/shared/modelos/registrar-prestamo-resultado';
import { FormularioPrestamo } from '../../formularios/shared/modelo/formulario-prestamo.model';
import { ActualizarFechaPagoFormularioComando } from "../../formularios/shared/modelo/actualizar-fecha-pago-formulario.comando";
import { BandejaConformarPrestamoResultado } from "../../prestamos-checklists/shared/modelos/bandeja-conformar-prestamo-resultado.model";
import { ProvidenciaComando } from "../../pagos/shared/modelo/providencia-comando.model";
import { FechaAprobacion } from "../../formularios/shared/modelo/fecha-aprobacion.model";
import { RegistrarRechazoReactivacionPrestamo } from '../../formularios/shared/modelo/registrar-rechazo-reactivacion-prestamo.model';
import { ReactivarPrestamoComando } from '../../formularios/shared/modelo/reactivar-prestamo-comando.model';
import { DatosPrestamoReactivacionResultado } from '../../formularios/shared/modelo/datos-prestamo-reactivacion-resultado.model';
import { MotivorechazoPrestamo } from '../../formularios/shared/modelo/motivo-rechazo-prestamo.model';
import { NumeroCajaComando } from '../../formularios/shared/modelo/numero-caja-comando.model';
import { ReporteResultado } from "../modelo/reporte-resultado.model";
import { GarantePrestamo } from '../modelo/garante-prestamo.mode';

@Injectable()
export class PrestamoService {
  private static filtrosConsulta: ConsultaBandejaPrestamos;
  private static consultaConformacionPrestamo: BandejaConformarPrestamoConsulta;
  private static formulariosResultadoConformacionPrestamo: BandejaConformarPrestamoResultado[];
  private static formulariosParaPrestamoConformacionPrestamo: FormularioPrestamo[];
  private static idsAgrupamientosConformarPrestamo: number[];
  private url = '/prestamos';

  constructor(private http: Http,
              private errorHandler: ErrorHandler) {
  }

  public consultarUsuariosCombo(): Observable<any> {
    return this.http.get('/usuarios/GetUsuariosParaCombo')
      .map((response) => response.json().resultado)
      .catch(this.errorHandler.handle);
  }

  public static guardarFiltros(filtros: ConsultaBandejaPrestamos): void {
    this.filtrosConsulta = filtros;
  }

  public static guardarFiltrosConformacionPrestamo(consulta: BandejaConformarPrestamoConsulta, formulariosResultado: BandejaConformarPrestamoResultado[], formulariosParaPrestamo: FormularioPrestamo[], idsAgrupamientosSeleccionados: number[]) {
    this.consultaConformacionPrestamo = consulta;
    this.formulariosResultadoConformacionPrestamo = formulariosResultado;
    this.formulariosParaPrestamoConformacionPrestamo = formulariosParaPrestamo;
    this.idsAgrupamientosConformarPrestamo = idsAgrupamientosSeleccionados;
  }

  public static recuperarFiltros(): ConsultaBandejaPrestamos {
    return this.filtrosConsulta;
  }

  public static recuperarFiltrosConformacionPrestamo(): any[] {
    let filtros = [];
    filtros.push(this.consultaConformacionPrestamo, this.formulariosResultadoConformacionPrestamo, this.formulariosParaPrestamoConformacionPrestamo, this.idsAgrupamientosConformarPrestamo);
    return filtros;
  }

  // TODO: preguntar si un solicitante solo puede tener un formulario por linea o un formulario unicamente
  public validarFormularioCanceladoParaGarante(garante, solicitantes, idLinea) {
    return this.http.post(`${this.url}/permitir-agrupacion-para-garantes-faltantes`, {
      garante,
      solicitantes,
      idLinea
    }).catch(this.errorHandler.handle);
  }

  public consultarBandejaPrestamo(comando: ConsultaBandejaPrestamos): Observable<Pagina<BandejaPrestamoResultado>> {
    return this.http.post(`${this.url}/consultar-bandeja`, comando)
      .map((res) => {
        return PaginaUtils.extraerPagina(BandejaPrestamoResultado, res);
      })
      .catch(this.errorHandler.handle);
  }
  public consultarTotalizador(consulta: ConsultaBandejaPrestamos) {
    return this.http.post(`${this.url}/consultar-totalizador`, consulta)
      .map((res) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public consultarBandejaConformarPrestamo(consulta: BandejaConformarPrestamoConsulta): Observable<Pagina<BandejaConformarPrestamoResultado>> {
    return this.http
      .get(this.url + '/consultar-conformar-prestamo', {search: HttpUtils.insertarPrefijo(consulta)})
      .map((res) => {
        return PaginaUtils.extraerPagina(BandejaConformarPrestamoResultado, res);
      })
      .catch(this.errorHandler.handle);
  }

  public consultarIntegrantes(id: number): Observable<IntegrantePrestamo[]> {
    return this.http.get(`${this.url + '/consulta-integrantes'}/${id}`)
      .map(PrestamoService.extraerIntegrantes)
      .catch(this.errorHandler.handle);
  }

  public consultarGarantesPrestamo(id: number): Observable<GarantePrestamo[]> {
    return this.http.get(`${this.url + '/consulta-datos-garante-prestamo'}/${id}`)
      .map(PrestamoService.extraerGarantes)
      .catch(this.errorHandler.handle);
  }

  public consultarRequisitos(id: number): Observable<RequisitoPrestamo[]> {
    let searchParams = new URLSearchParams();
    searchParams.set('id', id.toString());
    return this.http.get(`${this.url + '/consulta-requisitos'}/${id}`, {search: searchParams})
      .map(PrestamoService.extraerRequisitos)
      .catch(this.errorHandler.handle);
  }

  public aceptarChecklist(prestamo: Prestamo): Observable<void> {
    return this.http.post(this.url + '/aceptacion-requisitos', prestamo)
      .map((res) => res.json().resultado)
      .catch(this.errorHandler.handle);
  }

  public guardarChecklist(prestamo: Prestamo): Observable<void> {
    return this.http.post(this.url + '/guardado-requisitos', prestamo)
      .map((res) => res.json().resultado)
      .catch(this.errorHandler.handle);
  }

  public consultarRequisitosCargados(id: number, idFormularioLinea: number): Observable<RequisitoPrestamo[]> {
    let searchParams = new URLSearchParams();
    searchParams.set('id', id.toString());
    return this.http.get(`${this.url + '/consulta-requisitos-cargados'}/${id}/${idFormularioLinea}`, {search: searchParams})
      .map(PrestamoService.extraerRequisitos)
      .catch(this.errorHandler.handle);
  }

  private static extraerRequisitos(res: Response): RequisitoPrestamo[] {
    let resultado = res.json().resultado;
    return (resultado || []).map((value) => MapUtils.deserialize(RequisitoPrestamo, value));
  }

  private static extraerIntegrantes(res: Response): IntegrantePrestamo[] {
    let resultado = res.json().resultado;
    return (resultado || []).map((value) => MapUtils.deserialize(IntegrantePrestamo, value));
  }
  private static extraerGarantes(res: Response): GarantePrestamo[] {
    let resultado = res.json().resultado;
    return (resultado || []).map((value) => MapUtils.deserialize(GarantePrestamo, value));
  }

  public generarPrestamo(id): Observable<RegistrarPrestamoResultado> {
    return this.http.get(`${this.url}/generar-prestamo?id=${id}`)
      .map((res) => MapUtils.extractModel(RegistrarPrestamoResultado, res))
      .catch(this.errorHandler.handle);
  }

  public imprimirTxtGenerarPrestamo(idsAgrupamiento: string, generado: boolean): Observable<any> {
    return this.http.get(`${this.url}/imprimir-txt-generar-prestamo?idsAgrupamiento=${idsAgrupamiento}&generado=${generado}`)
      .map((res) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public consultarDatosPrestamo(id: number): Observable<Prestamo> {
    return this.http.get(`${this.url + '/consulta-datos-prestamo'}/${id}`)
      .map(PrestamoService.extraerPrestamo)
      .catch(this.errorHandler.handle);
  }

  public consultarSeguimientoPrestamo(consulta: ConsultaSeguimientosPrestamo): Observable<Pagina<SeguimientoPrestamo>> {
    return this.http.get(this.url + '/consulta-seguimientos', {search: HttpUtils.insertarPrefijo(consulta)})
      .map((res) => {
        return PaginaUtils.extraerPagina(SeguimientoPrestamo, res);
      })
      .catch(this.errorHandler.handle);
  }

  private static extraerPrestamo(res: Response): Prestamo {
    let resultado = res.json().resultado;
    if (resultado) {
      return MapUtils.deserialize(Prestamo, resultado);
    }
  }

  public rechazarPrestamo(comando: RechazarPrestamoComando): Observable<any> {
    return this.http
      .post(`${this.url}/rechazar-prestamo`, comando)
      .map((res) => {
        return res;
      }).catch(this.errorHandler.handle);
  }

  public generarPdfGrupoFamiliar(idFormularioLinea: number, idPrestamoRequisito: number): Observable<string> {
    let params = new URLSearchParams();
    params.set('id', idFormularioLinea.toString());
    return this.http.get(`/rentas/reporte-rentas?idFormularioLinea=${idFormularioLinea}&idPrestamoRequisito=${idPrestamoRequisito}`)
      .map((res: Response) => {
        return res.json().resultado;
      }).catch(this.errorHandler.handle);
  }

  public consultarEstadosPrestamo(): Observable<EstadoPrestamo[]> {
    return this.http.get(this.url + '/estados-prestamo')
      .map(PrestamoService.extraerEstadosPrestamo)
      .catch(this.errorHandler.handle);
  }

  private static extraerEstadosPrestamo(res: Response): EstadoPrestamo[] {
    let resultado = res.json().resultado;
    return (resultado || []).map((estado) => MapUtils.deserialize(EstadoPrestamo, estado));
  }

  public generarReporteSintysGrupoFamiliar(idFormularioLinea: number): Observable<string> {
    let params = new URLSearchParams();
    params.set('id', idFormularioLinea.toString());
    return this.http.get(`/sintys/reporte-sintys-prestamo?idFormularioLinea=${idFormularioLinea}`)
      .map((res: Response) => {
        return res.json().resultado;
      }).catch(this.errorHandler.handle);
  }

  public generarPdfHistorialRentas(idHistorial: number): Observable<string> {
    return this.http.get(`/rentas/historial-rentas?idHistorial=${idHistorial}`)
      .map((res: Response) => {
        return res.json().resultado;
      }).catch(this.errorHandler.handle);
  }

  public consultarEncabezadoPrestamo(id: number): Observable<EncabezadoPrestamo> {
    return this.http.get(`${this.url + '/encabezado-archivos'}/${id}`)
      .map(PrestamoService.extraerEncabezadoPrestamo)
      .catch(this.errorHandler.handle);
  }

  private static extraerEncabezadoPrestamo(res: Response): EncabezadoPrestamo {
    let resultado = res.json().resultado;
    if (resultado) {
      return MapUtils.deserialize(EncabezadoPrestamo, resultado);
    }
  }

  public obtenerFormulariosAgrupamiento(idAgrupamiento: number): Observable<FormularioPrestamo[]> {
    return this.http.get(`${this.url}/obtener-formularios-agrupamiento/${idAgrupamiento}`)
      .map((res) => MapUtils.extractModel(FormularioPrestamo, res))
      .catch(this.errorHandler.handle);
  }

  public actualizarFechaPagoFormulario(comando: ActualizarFechaPagoFormularioComando): Observable<object> {
    return this.http.post(this.url + '/actualizar-fecha-pago-formulario', comando)
      .map((res) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public generarReporteProvidencia(comando: ProvidenciaComando): Observable<ReporteResultado> {
    return this.http
      .get('/providencia/reporte-providencia-prestamo', {search: HttpUtils.insertarPrefijo(comando)})
      .map((res) => res.json().resultado)
      .catch(this.errorHandler.handle);
  }

  public obtenerFechaAprobacion(idPrestamo: number): Observable<FechaAprobacion> {
    return this.http.get(`${this.url}/obtener-fecha-aprobacion/${idPrestamo}`)
      .map((res) => MapUtils.extractModel(FechaAprobacion, res))
      .catch(this.errorHandler.handle);
  }

  public obtenerIdPrestamo(idFormulario: number): Observable<number> {
    return this.http.get(`${this.url}/obtener-id-prestamo/${idFormulario}`)
      .map((response) => response.json().resultado)
      .catch(this.errorHandler.handle);
  }

  public rechazarReactivacion(comando: RegistrarRechazoReactivacionPrestamo): Observable<object> {
    return this.http.post(this.url + '/registrar-rechazo-reactivacion-prestamo', comando)
      .map((res) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public reactivarPrestamo(comando: ReactivarPrestamoComando): Observable<object> {
    return this.http.post(this.url + '/registrar-reactivacion-prestamo', comando)
      .map((res) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public obtenerDatosReactivacion(idPrestamo: number): Observable<DatosPrestamoReactivacionResultado> {
    return this.http.get(`${this.url}/obtener-datos-reactivacion/${idPrestamo}`)
      .map((response) => response.json().resultado)
      .catch(this.errorHandler.handle);
  }

  public obtenerMotivosRechazoPrestamo(idPrestamo: number): Observable<MotivorechazoPrestamo[]> {
    return this.http.get(`${this.url}/obtener-motivos-rechazo-prestamo/${idPrestamo}`)
      .map((response) => response.json().resultado)
      .catch(this.errorHandler.handle);
  }

  public editarNumeroCaja(comando: NumeroCajaComando): Observable<object> {
    return this.http.post(this.url + '/editar-numero-caja', comando)
      .map((res) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public generarReporteBandejaExcel(consultaPrestamo: ConsultaBandejaPrestamos): Observable<any> {
    return this.http.post(`${this.url}/obtener-reporte-excel-bandeja-prestamos`, consultaPrestamo)
      .map((res) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }
  public generarReporteBandejaPDF(consultaPrestamo: ConsultaBandejaPrestamos): Observable<ReporteResultado> {
    return this.http
      .post(this.url + '/obtener-reporte-pdf-bandeja-prestamos', consultaPrestamo)
      .map((res) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }
}
