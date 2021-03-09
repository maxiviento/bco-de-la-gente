import { Injectable } from '@angular/core';
import { BandejaRecuperoConsulta } from './modelo/bandeja-recupero-consulta.model';
import { Pagina, PaginaUtils } from '../../shared/paginacion/pagina-utils';
import { Observable } from 'rxjs/Rx';
import { HttpUtils } from '../../shared/http-utils';
import { BandejaRecuperoResultado } from './modelo/bandeja-recupero-resultado.model';
import { Headers, Http, RequestOptions, Response } from '@angular/http';
import { ErrorHandler } from '../../core/http/error-handler.service';
import { MapUtils } from '../../shared/map-utils';
import { ImportarArchivoRecuperoResultado } from './modelo/importar-archivo-recupero-resultado.model';
import { ImportarArchivoResultadoBancoResultado } from './modelo/importar-archivo-resultado-banco-resultado.model';
import { BandejaResultadoBancoResultado } from './modelo/bandeja-resultado-banco-resultado.model';
import { InconsistenciaArchivoConsulta } from './modelo/inconsistencia-archivo-consulta.model';
import { VerImportacionArchivo } from './modelo/ver-importacion-archivo.model';
import { ComboEntidadesRecupero } from './modelo/combo-entidades-recupero.model';
import { Convenio } from "../../shared/modelo/convenio-model";

@Injectable()
export class RecuperoService {
  public url: string = '/recupero';
  public static filtrosConsultaRecupero: BandejaRecuperoConsulta;
  public static filtrosConsultaResultado: BandejaRecuperoConsulta;

  public static guardarFiltrosRecupero(filtros: BandejaRecuperoConsulta): void {
    this.filtrosConsultaRecupero = filtros;
  }
  public static recuperarFiltrosRecupero(): BandejaRecuperoConsulta {
    return this.filtrosConsultaRecupero;
  }

  public static guardarFiltrosResultado(filtros: BandejaRecuperoConsulta): void {
    this.filtrosConsultaResultado = filtros;
  }
  public static recuperarFiltrosResultado(): BandejaRecuperoConsulta {
    return this.filtrosConsultaResultado;
  }

  constructor(private http: Http,
              private errorHandler: ErrorHandler) {
  }

  public consultarBandeja(consulta: BandejaRecuperoConsulta): Observable<Pagina<BandejaRecuperoResultado>> {
    return this.http.get(`${this.url}/consultar-bandeja/`, {search: HttpUtils.insertarPrefijo(consulta)})
      .map((res) => {
        return PaginaUtils.extraerPagina(BandejaRecuperoResultado, res);
      })
      .catch(this.errorHandler.handle);
  }

  public registrarArchivoRecupero(comando: any): Observable<ImportarArchivoRecuperoResultado> {
    let headers = new Headers();
    let options = new RequestOptions({headers});
    let formData = HttpUtils.createFormData(comando);

    return this.http
      .post(`${this.url}/importar-archivo-recupero`, formData, options)
      .map((res) => MapUtils.extractModel(ImportarArchivoRecuperoResultado, res))
      .catch(this.errorHandler.handle);
  }

  public consultarBandejaResultadoBanco(consulta: BandejaRecuperoConsulta): Observable<Pagina<BandejaResultadoBancoResultado>> {
    return this.http.get(`${this.url}/consultar-bandeja-resultado-banco/`, {search: HttpUtils.insertarPrefijo(consulta)})
      .map((res) => {
        return PaginaUtils.extraerPagina(BandejaResultadoBancoResultado, res);
      })
      .catch(this.errorHandler.handle);
  }

  public registrarArchivoResultadoBanco(comando: any): Observable<ImportarArchivoResultadoBancoResultado> {
    let headers = new Headers();
    let options = new RequestOptions({headers});
    let formData = HttpUtils.createFormData(comando);

    return this.http
      .post(`${this.url}/importar-archivo-resultado-banco`, formData, options)
      .map((res) => MapUtils.extractModel(ImportarArchivoResultadoBancoResultado, res))
      .catch(this.errorHandler.handle);
  }

  public consultarInconsistenciaArchivoRecupero(consulta: InconsistenciaArchivoConsulta): Observable<Pagina<VerImportacionArchivo>> {
    return this.http.get(`${this.url}/consultar-inconsistencia-importacion-archivo-recupero/`, {search: HttpUtils.insertarPrefijo(consulta)})
      .map((res) => {
        return PaginaUtils.extraerPagina(VerImportacionArchivo, res);
      })
      .catch(this.errorHandler.handle);
  }

  public consultarInconsistenciaArchivoResultado(consulta: InconsistenciaArchivoConsulta): Observable<Pagina<VerImportacionArchivo>> {
    return this.http.get(`${this.url}/consultar-inconsistencia-importacion-archivo-resultado/`, {search: HttpUtils.insertarPrefijo(consulta)})
      .map((res) => {
        return PaginaUtils.extraerPagina(VerImportacionArchivo, res);
      })
      .catch(this.errorHandler.handle);
  }

  public consultarConveniosRecupero(): Observable<Convenio[]> {
    return this.http.get(`${this.url}/convenios-recupero`)
      .map((res) => MapUtils.extractModel(Convenio, res))
      .catch(this.errorHandler.handle);
  }

  public obtenerComboTipoEntidadRecupero(): Observable<ComboEntidadesRecupero[]> {
    return this.http.get(this.url + '/consultar-combo-entidades-recupero')
      .map(RecuperoService.extraerComboTipoEntidadRecupero)
      .catch(this.errorHandler.handle);
  }

  private static extraerComboTipoEntidadRecupero(res: Response): ComboEntidadesRecupero[] {
    let resultado = res.json().resultado;
    return (resultado || []).map((item) => MapUtils.deserialize(ComboEntidadesRecupero, item));
  }
}
