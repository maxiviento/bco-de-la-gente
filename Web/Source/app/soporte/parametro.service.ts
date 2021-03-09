import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import { Http, Response, URLSearchParams } from '@angular/http';
import { ErrorHandler } from '../core/http/error-handler.service';
import { Parametro } from './modelo/parametro.model';
import { MapUtils } from '../shared/map-utils';
import { Pagina, PaginaUtils } from '../shared/paginacion/pagina-utils';
import { LocalidadService } from '../shared/services/localidad.service';
import {ConsultaParametro} from "./modelo/consulta-parametro.model";
import {HttpUtils} from "../shared/http-utils";
import { VigenciaParametroConsulta } from '../shared/modelo/consultas/vigencia-parametro-consulta.model';
import { VigenciaParametro } from '../shared/modelo/vigencia-parametro.model';
import {VigenciaExistente} from "./modelo/vigencia-existente.model";

@Injectable()
export class ParametroService {
  private url = '/parametros';

  constructor(private http: Http,
              private errorHandler: ErrorHandler) {
  }

  public obtenerParametrosDetallado(consulta: ConsultaParametro): Observable<Pagina<Parametro>> {
    return this.http.get(`${this.url}/detallados`, {search: HttpUtils.insertarPrefijo(consulta)})
      .map((res) => {
        return PaginaUtils.extraerPagina(Parametro, res);
      }).catch(this.errorHandler.handle);

  }

  private configurarObtenerParametrosDetallados(idParametro?: number,
                                                incluirNoVigentes?: boolean): URLSearchParams {
    let params = new URLSearchParams();
    if (idParametro && idParametro != null) {
      params.set('id', idParametro.toLocaleString());
    }
      params.set('incluirNoVigentes', incluirNoVigentes.toLocaleString());
    return params;
  }


  public obtenerParametros(): Observable<Parametro[]> {
    return this.http.get(this.url)
      .map(ParametroService.extraerParametros)
      .catch(this.errorHandler.handle);
  }

  private static extraerParametros(res: Response): Parametro[] {
    return (res.json().resultado || [])
      .map((estados) => MapUtils.deserialize(Parametro, estados));
  }

  public existeVigencia(parametro: Parametro): Observable<VigenciaExistente> {
    return this.http.get(`${this.url}/existeVigencia`, {search:HttpUtils.insertarPrefijo(parametro)})
      .map(ParametroService.extraerVigenciaExistente)
      .catch(this.errorHandler.handle);
  }

  public modificarParametro(parametro: Parametro): Observable<void> {
    return this.http.put(`${this.url}/${parametro.id}`, parametro)
      .map((response) => response.json())
      .catch(this.errorHandler.handle);
  }

  public actualizarVigencia(parametro: Parametro): Observable<VigenciaParametro> {
    return this.http.post(this.url + '/actualizarVigencia', parametro)
      .map(ParametroService.extraerVigencia)
      .catch(this.errorHandler.handle);
  }

  public obtenerVigenciaParametro(vigenciaParametroConsulta: VigenciaParametroConsulta): Observable<VigenciaParametro> {
    return this.http.get(this.url + '/vigente', {search: HttpUtils.insertarPrefijo(vigenciaParametroConsulta)})
    .map(ParametroService.extraerVigencia)
      .catch(this.errorHandler.handle);
  }

  private static extraerVigencia(res: Response): VigenciaParametro {
    let resultado = res.json().resultado;
    if (resultado) {
      return MapUtils.deserialize(VigenciaParametro, resultado);
    }
  }

  private static extraerVigenciaExistente(res: Response): VigenciaExistente {
    let resultado = (res.json().resultado);
      if(resultado){
        return MapUtils.deserialize(VigenciaExistente, resultado);
      }
  }

}
