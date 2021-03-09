import { Injectable } from '@angular/core';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import 'rxjs/add/observable/throw';
import { Headers, Http, RequestOptions, Response } from '@angular/http';
import { Observable } from 'rxjs';
import { Etapa } from './modelo/etapa.model';
import { ConsultaEtapa } from './modelo/consulta-etapa.model';
import { ErrorHandler } from '../../core/http/error-handler.service';
import { MapUtils } from '../../shared/map-utils';
import { BajaEtapaComando } from './modelo/comando-baja-etapa.model';
import { EdicionEtapaComando } from './modelo/comando-edicion-etapa.model';
import { Pagina, PaginaUtils } from '../../shared/paginacion/pagina-utils';
import { HttpUtils } from '../../shared/http-utils';
import { LineaPrestamo } from '../../lineas/shared/modelo/linea-prestamo.model';

@Injectable()

export class EtapasService {
  public url: string = '/Etapas';

  private static filtrosConsulta: ConsultaEtapa;

  constructor(private http: Http, private errorHandler: ErrorHandler) {
  }

  public static guardarFiltros(filtros: ConsultaEtapa): void {
    this.filtrosConsulta = filtros;
  }

  public static recuperarFiltros(): ConsultaEtapa {
    return this.filtrosConsulta;
  }

  public registrarEtapa(etapa: Etapa): Observable<Etapa> {
    return this.http.post(this.url, etapa)
      .map(EtapasService.extraerEtapa)
      .catch(this.errorHandler.handle);
  }

  public darDeBajaEtapa(idEtapa: number, comando: BajaEtapaComando): Observable<any> {
    let parametrosComando = EtapasService.setParametrosComando(comando);

    return this.http.delete(`${this.url}/${idEtapa}`, {params: parametrosComando})
      .map((response) => response.json())
      .catch(this.errorHandler.handle);
  }

  public modificarEtapa(idEtapa: number, comando: EdicionEtapaComando): Observable<any> {
    return this.http.put(`${this.url}/${idEtapa}`, comando)
      .map((response) => response.json())
      .catch(this.errorHandler.handle);
  }

  public consultarEtapas(consulta: ConsultaEtapa): Observable<Pagina<Etapa>> {
    return this.http
      .get(this.url + '/consultar', {search: HttpUtils.insertarPrefijo(consulta)})
      .map((res) => {
        return PaginaUtils.extraerPagina(Etapa, res);
      })
      .catch(this.errorHandler.handle);
  }

  public consultarEtapa(id: number): Observable<Etapa> {
    return this.http
      .get(`${this.url}/${id}`)
      .map(EtapasService.extraerEtapa)
      .catch(this.errorHandler.handle);
  }

  private static extraerEtapa(res: Response): Etapa {
    let resultado = res.json().resultado;

    if (resultado) {
      return MapUtils.deserialize(Etapa, resultado);
    }
  }

  private static extraerEtapas(res: Response): Etapa [] {
    let resultado = res.json().resultado;
    return (resultado || []).map((etapa) => MapUtils.deserialize(Etapa, etapa));
  }

  public consultarEtapasCombo(): Observable<Etapa []> {
    return this.http.get(this.url + '/consultar-etapas')
      .map(EtapasService.extraerEtapas)
      .catch(this.errorHandler.handle);
  }

  public consultarEtapasPorPrestamo(idPrestamo: number): Observable<Etapa []> {
    return this.http.get(this.url + '/consultar-etapas-prestamo?idPrestamo=' + idPrestamo)
      .map(EtapasService.extraerEtapas)
      .catch(this.errorHandler.handle);
  }

  private static setParametrosConsulta(consulta: ConsultaEtapa): object {
    let params = {};
    Object.getOwnPropertyNames(consulta)
      .map((key: string) => {
        params['consulta.' + key] = consulta[key];
      });
    return params;
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
