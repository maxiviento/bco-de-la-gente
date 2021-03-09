import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Http } from '@angular/http';
import { ErrorHandler } from '../core/http/error-handler.service';
import { MapUtils } from '../shared/map-utils';
import { TablaDefinida } from './modelo/tabla-definida.model';
import { ConsultaParametrosTablasDefinidas } from './modelo/consulta-parametros-tablas-definidas';
import { ParametroTablaDefinida } from './modelo/parametro-tabla-definida';
import { HttpUtils } from '../shared/http-utils';
import { RechazarParametroTablaDefinida } from './modelo/rechazar-parametro-tabla-definida';
import { Pagina, PaginaUtils } from '../shared/paginacion/pagina-utils';
import { BandejaFormularioResultado } from '../formularios/shared/modelo/bandeja-formulario-resultado.model';
import { ConsultaTablasDefinidas } from './modelo/consulta-tablas-definidas';

@Injectable()
export class TablasDefinidasService {
  private url = '/tablasdefinidas';
  private static filtrosConsulta: ConsultaParametrosTablasDefinidas;
  private static filtrosTablasDefinidas: ConsultaTablasDefinidas;

  constructor(private http: Http,
              private errorHandler: ErrorHandler) {
  }

  public static guardarFiltrosParametros(filtros: ConsultaParametrosTablasDefinidas): void {
    this.filtrosConsulta = filtros;
  }

  public static recuperarFiltrosParametros(): ConsultaParametrosTablasDefinidas {
    return this.filtrosConsulta;
  }

  public static guardarFiltrosTablasDefinidas(filtros: ConsultaTablasDefinidas) {
    this.filtrosTablasDefinidas = filtros;
  }

  public static recuperarFiltrosTablasDefinidsa(): ConsultaTablasDefinidas {
    return this.filtrosTablasDefinidas;
  }

  public obtenerTablasDefinidas(): Observable<TablaDefinida[]> {
    return this.http.get(this.url)
      .map((res) => MapUtils.extractModel(TablaDefinida, res))
      .catch(this.errorHandler.handle);
  }

  public obtenerTablasDefinidasFiltradas(consulta: ConsultaTablasDefinidas): Observable<Pagina<TablaDefinida>> {
    return this.http.get(this.url + '/consultar-tablas-paginadas', {search: HttpUtils.insertarPrefijo(consulta)})
      .map((res) => {
        return PaginaUtils.extraerPagina(TablaDefinida, res);
      })
      .catch(this.errorHandler.handle);
  }

  public obtenerDatosTablaDefinida(idTabla: number): Observable<TablaDefinida> {
    return this.http.get(`${this.url}/consultar-tabla/${idTabla}`)
      .map((res) => MapUtils.extractModel(TablaDefinida, res))
      .catch(this.errorHandler.handle);
  }

  public obtenerParametrosCombo(consulta: ConsultaParametrosTablasDefinidas): Observable<ParametroTablaDefinida[]> {
    return this.http.get(this.url + '/consultar-parametros-combo', {search: HttpUtils.insertarPrefijo(consulta)})
      .map((res) => MapUtils.extractModel(ParametroTablaDefinida, res))
      .catch(this.errorHandler.handle);
  }

  public obtenerParametrosFiltrados(consulta: ConsultaParametrosTablasDefinidas): Observable<Pagina<ParametroTablaDefinida>> {
    return this.http.get(this.url + '/consultar-parametros', {search: HttpUtils.insertarPrefijo(consulta)})
      .map((res) => {
        return PaginaUtils.extraerPagina(ParametroTablaDefinida, res);
      })
      .catch(this.errorHandler.handle);
  }

  public rechazarParametro(comando: RechazarParametroTablaDefinida): Observable<any> {
    return this.http
      .post(`${this.url}/rechazar-parametro`, comando)
      .map((res) => {
        return res;
      }).catch(this.errorHandler.handle);
  }

  public registrarParametro(parametro: ParametroTablaDefinida, idTabla: number): Observable<any> {
    return this.http
      .post(`${this.url}/registrar-parametro/${idTabla}`, parametro)
      .map((res) => {
        return res.json().resultado;
      }).catch(this.errorHandler.handle);
  }

  public consultarParametro(id: number): Observable<TablaDefinida> {
    return this.http
      .get(`${this.url}/parametro/${id}`)
      .map((res) => MapUtils.extractModel(TablaDefinida, res))
      .catch(this.errorHandler.handle);
  }

}
