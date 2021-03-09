import { Injectable } from '@angular/core';
import { ErrorHandler } from '../../core/http/error-handler.service';
import { Headers, Http, RequestOptions, Response } from '@angular/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { MapUtils } from '../map-utils';
import { LineaPrestamo } from '../../lineas/shared/modelo/linea-prestamo.model';
import { InformacionLinea } from '../../formularios/shared/modelo/informacionLinea.model';
import { Cuadrante } from '../../formularios/shared/modelo/cuadrante.model';
import { Pagina, PaginaUtils } from '../paginacion/pagina-utils';
import { HttpUtils } from '../http-utils';
import { DetalleLineaPrestamo } from '../../lineas/shared/modelo/detalle-linea-prestamo.model';
import { RequisitosResultado } from '../../lineas/shared/modelo/resultado-requisitos.model';
import { BajaComando } from '../../lineas/shared/modelo/baja-comando.model';
import { RequisitoCuadrante } from '../../formularios/shared/modelo/requisito-cuadrante.model';
import { LineaCombo } from '../../formularios/shared/modelo/linea-combo.model';
import { Convenio } from '../modelo/convenio-model';
import { Localidad } from '../domicilio-linea/localidad.model';
import { DetalleLineaCombo } from '../../lineas/shared/modelo/detalle-linea-combo.model';
import { OngComboResultado } from '../../formularios/shared/modelo/ong.model';
import { OngLinea } from "../../lineas/shared/modelo/ong-linea.model";
import { ModificacionOngLineaComando } from "../../lineas/shared/modelo/modificacion-ong-linea-comando.model";

@Injectable()

export class LineaService {
  private static filtrosConsulta: LineaPrestamo;

  private url = '/lineasprestamo';
  private obsArray: BehaviorSubject<Localidad []> = new BehaviorSubject<Localidad []>(undefined);

  constructor(private http: Http,
              private errorHandler: ErrorHandler) {
  }

  public static guardarFiltros(filtros: LineaPrestamo): void {
    this.filtrosConsulta = filtros;
  }

  public static recuperarFiltros(): LineaPrestamo {
    return this.filtrosConsulta;
  }

  public consultarLineas(): Observable<LineaPrestamo[]> {
    return this.http.get(this.url)
      .map(LineaService.extraerLineas)
      .catch(this.errorHandler.handle);
  }

  public consultarLineasParaCombo(multiple?: boolean): Observable<LineaCombo[]> {
    return this.http.get(this.url + '/GetLineasParaCombo' + '?multiple=' + !!multiple)
      .map(LineaService.extraerLineasParaCombo)
      .catch(this.errorHandler.handle);
  }

  public consultarCuadrantesDisponibles(): Observable<Cuadrante []> {
    return this.http.get(this.url + '/consulta-cuadrantes')
      .map(LineaService.extraerCuadrantes)
      .catch(this.errorHandler.handle);
  }

  // Obtiene cuadrantes por id linea. usado desde configuracion de formularios
  public consultarCuadrantesLinea(id: number): Observable<Cuadrante[]> {
    return this.http.get(`${this.url}/cuadrantes-id-linea/${id}`)
      .map(LineaService.extraerCuadrantes)
      .catch(this.errorHandler.handle);
  }

  private static extraerCuadrantes(res: Response): Cuadrante[] {
    let resultado = res.json().resultado;
    return (resultado || []).map((linea) => MapUtils.deserialize(Cuadrante, linea));
  }

  private static extraerLineas(res: Response): LineaPrestamo[] {
    let resultado = res.json().resultado;
    return (resultado || []).map((linea) => MapUtils.deserialize(LineaPrestamo, linea));
  }

  private static extraerLineasParaCombo(res: Response): LineaCombo[] {
    let resultado = res.json().resultado;
    return (resultado || []).map((linea) => MapUtils.deserialize(LineaCombo, linea));
  }

  private static extraerCuadrantesFormulario(res: Response): Cuadrante[] {
    let resultado = res.json().resultado;
    if (resultado) {
      return (resultado || []).map((cuadrante) => MapUtils.deserialize(Cuadrante, cuadrante));
    }
  }

  // Obtiene cuadrantes por id detalle. usado desde formulario
  public consultarCuadrantesFormulario(id: number): Observable<Cuadrante[]> {
    return this.http.get(`${this.url}/cuadrantes/${id}`)
      .map(LineaService.extraerCuadrantesFormulario)
      .catch(this.errorHandler.handle);
  }

  public darDeBajaLinea(comando: BajaComando): Observable<LineaPrestamo> {
    let parametrosComando = LineaService.setParametrosComando(comando);
    return this.http.delete(`${this.url}`, {params: parametrosComando})
      .map((res) => MapUtils.extractModel(LineaPrestamo, res))
      .catch(this.errorHandler.handle);
  }

  public darDeBajaDetalleLinea(comando: BajaComando): Observable<DetalleLineaPrestamo> {
    let parametrosComando = LineaService.setParametrosComando(comando);
    return this.http.delete(`${this.url}/baja-detalle/`, {params: parametrosComando})
      .map((res) => MapUtils.extractModel(DetalleLineaPrestamo, res))
      .catch(this.errorHandler.handle);
  }

  public asignarLocalidad(localidades: Localidad[]) {
    this.obsArray.next(localidades);
  }

  public obtenerLocalidades(): Observable<Localidad[]> {
    return this.obsArray.asObservable();
  }

  private static setParametrosComando(comando: any): Object {
    let params = {};
    Object.getOwnPropertyNames(comando)
      .map((key: string) => {
        params['comando.' + key] = comando[key];
      });
    return params;
  }

  public registrarLinea(linea: LineaPrestamo): Observable<number> {
    let headers = new Headers();
    let options = new RequestOptions({headers});
    let formData = HttpUtils.createFormData(linea);

    return this.http
      .post(this.url, formData, options)
      .map((response) => response.json().resultado)
      .catch(this.errorHandler.handle);
  }

  public consultarLineasPorFiltros(consultaLineaPrestamo: LineaPrestamo): Observable<Pagina<LineaPrestamo>> {
    return this.http
      .get(this.url + '/consultar', {search: HttpUtils.insertarPrefijo(consultaLineaPrestamo)})
      .map((res) => PaginaUtils.extraerPagina(LineaPrestamo, res))
      .catch(this.errorHandler.handle);
  }

  public consultarDetalleLineaPorIdLinea(consultaDetalleLinea: DetalleLineaPrestamo): Observable<Pagina<DetalleLineaPrestamo[]>> {
    return this.http
      .get(this.url + '/consultar/detalle/grilla', {search: HttpUtils.insertarPrefijo(consultaDetalleLinea)})
      .map((res) => PaginaUtils.extraerPagina(DetalleLineaPrestamo, res))
      .catch(this.errorHandler.handle);
  }

  public registrarConfiguracion(linea: InformacionLinea): Observable<string> {
    return this.http.post(this.url + '/configurar-orden-cuadrantes', linea)
      .map(LineaService.extraerConfiguracionLinea)
      .catch(this.errorHandler.handle);
  }

  public consultarLineaPorId(idLinea: number): Observable<LineaPrestamo> {
    return this.http.get(`${this.url}/consultar/${idLinea}`)
      .map((res) => MapUtils.extractModel(LineaPrestamo, res))
      .catch(this.errorHandler.handle);
  }

  public consultarLocalidadesLineaPorId(idLinea: number): Observable<Localidad[]> {
    return this.http.get(`${this.url}/consultar-localidades/${idLinea}`)
      .map((res: Response) => {
        return res.json().resultado;
      }).catch(this.errorHandler.handle);
  }

  public obtenerDetallesLineaCombo(idLinea: number): Observable<DetalleLineaCombo[]> {
    return this.http.get(`${this.url}/obtener-detalles-linea-combo/${idLinea}`)
      .map((res) => MapUtils.extractModel(DetalleLineaCombo, res))
      .catch(this.errorHandler.handle);
  }

  public consultarDetalleLineaPorIdDetalle(idDetalle: number): Observable<DetalleLineaPrestamo> {
    return this.http.get(`${this.url}/consultar/detalle/${idDetalle}`)
      .map(LineaService.extraerDetalle)
      .catch(this.errorHandler.handle);
  }

  private static extraerDetalle(response: Response) {
    let resultado = response.json().resultado;
    if (resultado) {
      return MapUtils.deserialize(DetalleLineaPrestamo, resultado);
    }
  }

  public consultarDetallePorIdLineaSinPaginar(idLinea: number): Observable<DetalleLineaPrestamo[]> {
    return this.http.get(`${this.url}/consultar/detalles/${idLinea}`)
      .map((res) => MapUtils.extractModel(DetalleLineaPrestamo, res))
      .catch(this.errorHandler.handle);
  }

  public consultarRequisitosPorLinea(idLinea: number): Observable<RequisitosResultado[]> {
    return this.http
      .get(`${this.url}/consultar/requisitos/${idLinea}`)
      .map(LineaService.extraerRequisitos)
      .catch(this.errorHandler.handle);
  }

  public consultarOngPorLinea(idLinea: number): Observable<OngLinea[]> {
    return this.http.get(`${this.url}/consultar/ong/${idLinea}`)
      .map((res) => MapUtils.extractModel(OngLinea, res))
      .catch(this.errorHandler.handle);
  }

  private static extraerRequisitos(res: Response): RequisitosResultado[] {
    let resultado = res.json().resultado;
    if (resultado) {
      return (resultado || []).map((requisito) => MapUtils.deserialize(RequisitosResultado, requisito));
    }
  }

  private static extraerConfiguracionLinea(res: Response): InformacionLinea {
    let resultado = res.json().resultado;
    if (resultado) {
      return MapUtils.deserialize(InformacionLinea, resultado);
    }
  }

  public modificarLinea(linea: LineaPrestamo): Observable<LineaPrestamo> {
    let headers = new Headers();
    let options = new RequestOptions({headers});
    let formData = HttpUtils.createFormData(linea);
    return this.http.put(this.url, formData, options)
      .map((res) => MapUtils.extractModel(LineaPrestamo, res))
      .catch(this.errorHandler.handle);
  }

  public modificarDetalle(detalle: DetalleLineaPrestamo): Observable<DetalleLineaPrestamo> {
    return this.http.put(this.url + '/actualizar-detalle', detalle)
      .map((res) => MapUtils.extractModel(DetalleLineaPrestamo, res))
      .catch(this.errorHandler.handle);
  }

  public consultarRequisitosLineaParaCuadrante(idLinea: number): Observable<RequisitoCuadrante[]> {
    return this.http
      .get(`${this.url}/consultar/requisitosParaCuadrante/${idLinea}`)
      .map(LineaService.extraerRequisitosCuadrante)
      .catch(this.errorHandler.handle);
  }

  private static extraerRequisitosCuadrante(res: Response): RequisitoCuadrante[] {
    let resultado = res.json().resultado;
    if (resultado) {
      return (resultado || []).map((requisito) => MapUtils.deserialize(RequisitoCuadrante, requisito));
    }
  }

  public consultarNombresLineas(): Observable<LineaCombo[]> {
    return this.http.get(`${this.url}/consultar-nombres-lineas/`)
      .map(LineaService.extraerLineasParaCombo)
      .catch(this.errorHandler.handle);
  }

  public consultarConvenios(): Observable<Convenio[]> {
    return this.http.get(`${this.url}/convenios`)
      .map((res) => MapUtils.extractModel(Convenio, res))
      .catch(this.errorHandler.handle);
  }

  public obtenerOngs(): Observable<OngComboResultado[]> {
    return this.http.get(`${this.url}/obtener-ongs`)
      .map((res) => MapUtils.extractModel(OngComboResultado, res))
      .catch(this.errorHandler.handle);
  }

  public obtenerOngsPorNombre(nombre: string): Observable<OngComboResultado[]> {
    return this.http.get(`${this.url}/obtener-ongs-por-nombre/${nombre}`)
      .map((res) => MapUtils.extractModel(OngComboResultado, res))
      .catch(this.errorHandler.handle);
  }

  public modificarOngLinea(comando: ModificacionOngLineaComando): Observable<boolean> {
    return this.http
      .post(`${this.url}/modificar-ong-linea`, comando)
      .map((response) => response.json().resultado)
      .catch(this.errorHandler.handle);
  }
}
