import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Http, Response } from '@angular/http';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

import { ErrorHandler } from '../../core/http/error-handler.service';

import { ConsultaSituacionPersonas } from './modelos/consulta-situacion-personas.model';
import { Pagina, PaginaUtils } from '../../shared/paginacion/pagina-utils';
import { SituacionPersonasResultado } from './modelos/situacion-personas-resultado.model';
import { HttpUtils } from '../../shared/http-utils';
import { FormulariosSituacionResultado } from './modelos/formularios-situacion-resultado.model';
import { MotivoRechazoReferenciaConsulta } from '../../shared/modelo/consultas/motivo-rechazo-referencia-consulta.model';
import { MotivoRechazoReferencia } from '../../shared/modelo/motivo-rechazo-referencia.model';
import { MapUtils } from '../../shared/map-utils';
import { Archivo } from '../../shared/modelo/archivo.model';
import { SituacionPersonasResultadoVista } from './modelos/situacion-personas-resultado-vista.model';

@Injectable()
export class SituacionPersonaService {
  private static filtrosConsulta: ConsultaSituacionPersonas;
  public url: string = '/Formularios';

  constructor(private http: Http, private errorHandler: ErrorHandler) {
  }

  public consultarSituacionPersona(consulta: ConsultaSituacionPersonas): Observable<Pagina<SituacionPersonasResultado>> {
    return this.http
      .get( this.url + '/obtener-situacion-persona', {search: HttpUtils.insertarPrefijo(consulta)})
      .map( (res) => {
        return PaginaUtils.extraerPagina(SituacionPersonasResultado, res);
        })
      .catch(this.errorHandler.handle);
  }
  public consultarSituacionPersonaVista(consulta: ConsultaSituacionPersonas): Observable<SituacionPersonasResultadoVista[]> {
    return this.http
      .get( this.url + '/obtener-vista-situacion-persona', {search: HttpUtils.insertarPrefijo(consulta)})
      .map( (res) => MapUtils.extractModel(SituacionPersonasResultadoVista, res))
      .catch(this.errorHandler.handle);
  }
  public consultarFormulariosSituacionPersona(consulta: ConsultaSituacionPersonas): Observable<Pagina<FormulariosSituacionResultado>> {
    return this.http
      .get( this.url + '/obtener-formularios-situacion-persona', {search: HttpUtils.insertarPrefijo(consulta)})
      .map( (res) => PaginaUtils.extraerPagina(FormulariosSituacionResultado, res))
      .catch(this.errorHandler.handle);
  }

  public consultarMotivosRechazo(consulta: MotivoRechazoReferenciaConsulta): Observable<MotivoRechazoReferencia[]> {
    return this.http
      .get( this.url + '/obtener-motivos-rechazo-referencia', {search: HttpUtils.insertarPrefijo(consulta)})
      .map((res: Response) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public consultarMotivosRechazoPorFormulario(idFormularios: string): Observable<MotivoRechazoReferencia[]> {
    return this.http
      .get( this.url + '/obtener-motivos-rechazo-por-formulario', {search: HttpUtils.insertarPrefijo(idFormularios)})
      .map((res: Response) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public obtenerReporteSituacionPersona(consulta: ConsultaSituacionPersonas): Observable<Archivo> {
    return this.http
      .get( this.url + '/obtener-reporte-excel-situacion-persona', {search: HttpUtils.insertarPrefijo(consulta)})
      .map( (res) => MapUtils.extractModel(Archivo, res))
      .catch(this.errorHandler.handle);
  }

  public static guardarFiltros(filtros: ConsultaSituacionPersonas): void {
    this.filtrosConsulta = filtros;
  }

  public static recuperarFiltros(): ConsultaSituacionPersonas {
    return this.filtrosConsulta;
  }
}
