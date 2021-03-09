import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

import { MapUtils } from '../../shared/map-utils';
import { ErrorHandler } from '../../core/http/error-handler.service';

import { Area } from './modelo/area.model';
import { ConsultaArea } from './modelo/consulta-area.model';
import { EdicionAreaComando } from './modelo/edicion-area.model';
import { BajaAreaComando } from './modelo/baja-area.model';
import { Pagina, PaginaUtils } from '../../shared/paginacion/pagina-utils';
import { HttpUtils } from '../../shared/http-utils';
import { ReporteResultado } from "../../shared/modelo/reporte-resultado.model";

@Injectable()
export class AreasService {
  private static filtrosConsulta: ConsultaArea;

  private static extraerArea(response: Response) {
    let resultado = response.json().resultado;
    if (resultado) {
      return MapUtils.deserialize(Area, resultado);
    }
  }

  public url: string = '/Areas';

  constructor(private http: Http, private errorHandler: ErrorHandler) {
  }

  public registrarArea(area: Area): Observable<Area> {
    return this.http
      .post(this.url, area)
      .map(AreasService.extraerArea)
      .catch(this.errorHandler.handle);
  }

  public static guardarFiltros(filtros: ConsultaArea): void {
    this.filtrosConsulta = filtros;
  }

  public static recuperarFiltros(): ConsultaArea {
    return this.filtrosConsulta;
  }

  public consultarAreas(consulta: ConsultaArea): Observable<Pagina<Area>> {
    return this.http
      .get(this.url + '/consultar', {search: HttpUtils.insertarPrefijo(consulta)})
      .map((res) => {
        return PaginaUtils.extraerPagina(Area, res);
      })
      .catch(this.errorHandler.handle);
  }

  public consultarArea(id: number): Observable<Area> {
    return this.http
      .get(`${this.url}/${id}`)
      .map(AreasService.extraerArea)
      .catch(this.errorHandler.handle);
  }

  public consultarAreasCombo(): Observable<Area []> {
    return this.http.get(this.url + '/consultar-areas')
      .map(AreasService.extraerAreas)
      .catch(this.errorHandler.handle);
  }

  private static extraerAreas(res: Response): Area [] {
    let resultado = res.json().resultado;
    return (resultado || []).map((area) => MapUtils.deserialize(Area, area));
  }


  public modificarArea(idArea: number, comando: EdicionAreaComando): Observable<any> {
    return this.http.put(`${this.url}/${idArea}`, comando)
      .map((response) => response.json())
      .catch(this.errorHandler.handle);
  }

  public darDeBajaArea(idArea: number, comando: BajaAreaComando): Observable<any> {
    let parametrosComando = AreasService.setParametrosComando(comando);

    return this.http.delete(`${this.url}/${idArea}`, {params: parametrosComando})
      .map((response) => response.json())
      .catch(this.errorHandler.handle);
  }

  private static setParametrosComando(comando: any): object {
    let params = {};
    Object.getOwnPropertyNames(comando)
      .map((key: string) => {
        params['comando.' + key] = comando[key];
      });
    return params;
  }
}
