import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { ErrorHandler } from '../../../core/http/error-handler.service';
import { MontoDisponible } from './modelo/monto-disponible.model';
import { Observable } from 'rxjs/Observable';
import { MapUtils } from '../../../shared/map-utils';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import { BajaMontoDisponibleComando } from './modelo/baja-monto-disponible.model';
import { EdicionMontoDisponibleComando } from './modelo/edicion-monto-disponible.model';
import { IdMontoDisponibleResultado } from './modelo/id-monto-disponible-resultado.model';
import { BandejaMontoDisponibleConsulta } from './modelo/consulta-bandeja-monto-disponible.model';
import { BandejaMontoDisponibleResultado } from './modelo/resultado-bandeja-monto-disponible.model';
import { Pagina, PaginaUtils } from '../../../shared/paginacion/pagina-utils';
import { HttpUtils } from '../../../shared/http-utils';
import { ItemCombo } from '../../../shared/modelo/item-combo.model';
import { MovimientosMontoDisponible } from './modelo/movimientos-monto-disponible.model';
import { MovimientoMontoConsulta } from './modelo/movimiento-monto-consulta.model';

@Injectable()
export class MontoDisponibleService {
  private url = '/montodisponible';

  public static filtrosConsulta: BandejaMontoDisponibleConsulta;

  public static guardarFiltros(filtros: BandejaMontoDisponibleConsulta): void {
    this.filtrosConsulta = filtros;
  }

  public static recuperarFiltros(): BandejaMontoDisponibleConsulta {
    return this.filtrosConsulta;
  }

  constructor(private http: Http,
              private errorHandler: ErrorHandler) {
  }

  public registrar(montoDisponible: MontoDisponible): Observable<IdMontoDisponibleResultado> {
    return this.http
      .post(this.url, montoDisponible)
      .map(MontoDisponibleService.extraerIdMontoDisponibleResultado)
      .catch(this.errorHandler.handle);
  }

  private static extraerMontoDisponible(response: Response) {
    let resultado = response.json().resultado;
    if (resultado) {
      return MapUtils.deserialize(MontoDisponible, resultado);
    }
  }

  private static extraerIdMontoDisponibleResultado(response: Response) {
    let resultado = response.json().resultado;
    if (resultado) {
      return MapUtils.deserialize(IdMontoDisponibleResultado, resultado);
    }
  }

  public darDeBaja(idMontoDisponible: number, comando: BajaMontoDisponibleComando): Observable<any> {
    let parametros = MontoDisponibleService.setParametrosComando(comando);
    return this.http.delete(`${this.url}/${idMontoDisponible}`, {params: parametros})
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

  public modificar(idMontoDisponible: number, comando: EdicionMontoDisponibleComando): Observable<IdMontoDisponibleResultado> {
    return this.http.put(`${this.url}/${idMontoDisponible}`, comando)
      .map(MontoDisponibleService.extraerIdMontoDisponibleResultado)
      .catch(this.errorHandler.handle);
  }

  public obtenerPorId(idMontoDisponible: number): Observable<any> {
    return this.http.get(`${this.url}/${idMontoDisponible}`)
      .map((res) => MapUtils.extractModel(MontoDisponible, res))
      .catch(this.errorHandler.handle);
  }
  public obtenerPorNro(nroMontoDisponible: number): Observable<any> {
    return this.http.get(`${this.url}/obtener-por-numero?nroMonto=${nroMontoDisponible}`)
      .map((res) => MapUtils.extractModel(MontoDisponible, res))
      .catch(this.errorHandler.handle);
  }

  public consultarBandeja(consulta: BandejaMontoDisponibleConsulta): Observable<Pagina<BandejaMontoDisponibleResultado>> {
    return this.http.get(`${this.url}/consultar/` , {search: HttpUtils.insertarPrefijo(consulta)})
      .map((res) => {
        return PaginaUtils.extraerPagina(BandejaMontoDisponibleResultado, res);
      })
      .catch(this.errorHandler.handle);
  }

  public obtenerMovimientosMonto(consulta: MovimientoMontoConsulta) {
    return this.http.get(`${this.url}/obtener-movimientos/` , {search: HttpUtils.insertarPrefijo(consulta)})
      .map((res) => {
        return PaginaUtils.extraerPagina(MovimientosMontoDisponible, res);
      })
      .catch(this.errorHandler.handle);
  }

  public consultarMontosParaCombo(): Observable<ItemCombo[]> {
    return this.http.get(this.url + '/consultar-combo')
      .map((res) => MapUtils.extractModel(ItemCombo, res))
      .catch(this.errorHandler.handle);
  }
}
