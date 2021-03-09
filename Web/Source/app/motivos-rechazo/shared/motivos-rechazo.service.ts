import { Injectable } from '@angular/core';
import { ConsultaMotivosRechazo } from './modelo/consulta-motivos-rechazo.model';
import { ErrorHandler } from '../../core/http/error-handler.service';
import { Observable } from 'rxjs';
import { MotivoRechazo } from '../../formularios/shared/modelo/motivo-rechazo';
import { MapUtils } from '../../shared/map-utils';
import { Pagina, PaginaUtils } from '../../shared/paginacion/pagina-utils';
import { HttpUtils } from '../../shared/http-utils';
import { BajaMotivoRechazoComando } from './modelo/comando-baja-motivo-rechazo.model';
import { Ambito } from './modelo/ambito.model';
import { Http, Response } from '@angular/http';
import { MotivoRechazoAbreviatura } from './modelo/motivos-rechazo-abreviatura.model';
import {  ModificarMotivoRechazoComando } from '../../formularios/shared/modelo/modificar-motivo-rechazo-comando.model';

@Injectable()
export class MotivosRechazoService {
  private url = '/motivosrechazo';

  private static filtrosConsulta: ConsultaMotivosRechazo;

  constructor(private http: Http,
              private errorHandler: ErrorHandler) {
  }

  public static guardarFiltros(filtros: ConsultaMotivosRechazo): void {
    this.filtrosConsulta = filtros;
  }

  public static recuperarFiltros(): ConsultaMotivosRechazo {
    return this.filtrosConsulta;
  }

  public consultarMotivosRechazo(): Observable<MotivoRechazo[]> {
    return this.http.get(this.url)
      .map((res) => MapUtils.extractModel(MotivoRechazo, res))
      .catch(this.errorHandler.handle);
  }

  public consultarMotivosRechazoPaginados(consulta: ConsultaMotivosRechazo): Observable<Pagina<MotivoRechazo[]>> {
    return this.http.get(this.url + '/consultar', {search: HttpUtils.insertarPrefijo(consulta)})
      .map((res) => PaginaUtils.extraerPagina(MotivoRechazo, res))
      .catch(this.errorHandler.handle);
  }

  public registrarMotivoRechazo(motivo: MotivoRechazo): Observable<MotivoRechazo> {
    return this.http.post(this.url, motivo)
      .map((res) => MapUtils.extractModel(MotivoRechazo, res))
      .catch(this.errorHandler.handle);
  }

  public consultarMotivoRechazo(idMotivo: number, idAmbito): Observable<MotivoRechazo> {
    return this.http.get(this.url + '/detalle' + `/${idMotivo}` + `/${idAmbito}`)
      .map((res) => MapUtils.extractModel(MotivoRechazo, res))
      .catch(this.errorHandler.handle);
  }

  public editarMotivoRechazo(motivoComando: ModificarMotivoRechazoComando): Observable<any> {
    return this.http.post(this.url + '/modificar', motivoComando)
      .map((response) =>
        response.json().resultado)
      .catch(this.errorHandler.handle);
  }

  public darDeBajaMotivoRechazo(comando: BajaMotivoRechazoComando): Observable<any> {
    return this.http.post(this.url + '/baja', comando)
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

  public consultarAmbitos(): Observable<Ambito[]> {
    return this.http.get(this.url + '/ambitos')
      .map(MotivosRechazoService.extraerAmbitos)
      .catch(this.errorHandler.handle);
  }

  private static extraerAmbitos(res: Response): Ambito[] {
    let resultado = res.json().resultado;
    return (resultado || []).map((ambito) => MapUtils.deserialize(Ambito, ambito));
  }

  public consultarAbreviaturas(): Observable<MotivoRechazoAbreviatura[]> {
    return this.http.get(this.url + '/obtener-abreviaturas')
      .map((res) => MapUtils.extractModel(MotivoRechazoAbreviatura, res))
      .catch(this.errorHandler.handle);
  }

  public consultarMotivosRechazoPorAmbito(idAmbito: number): Observable<MotivoRechazo[]> {
    return this.http.get(this.url + '/motivos-ambito' + `/${idAmbito}`)
      .map((res) => {
        return MapUtils.extractModel(MotivoRechazo, res);
      })
      .catch(this.errorHandler.handle);
  }

  public verificarAbreviaturaExistente(idAmbito: number, abreviatura: string): Observable<boolean> {
    return this.http.get(this.url + '/verificar-abreviatura' + `/${idAmbito}` + `/${abreviatura}`)
      .map((respuesta) => {
        return respuesta.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public verificarNombreExistente(idAmbito: number, nombre: string): Observable<boolean> {
    return this.http.get(this.url + '/verificar-nombre' + `/${idAmbito}` + `/${nombre}`)
      .map((respuesta) => {
        return respuesta.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public verificarCodigoExistente(idAmbito: number, codigo: string): Observable<boolean> {
    return this.http.get(this.url + '/verificar-codigo' + `/${idAmbito}` + `/${codigo}`)
      .map((respuesta) => {
        return respuesta.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }
}
