import { Injectable } from '@angular/core';
import { ErrorHandler } from '../../core/http/error-handler.service';
import { Http } from '@angular/http';
import { Observable } from 'rxjs';
import { MapUtils } from '../../shared/map-utils';
import { OrigenPrestamo } from './modelo/origen-prestamo.modelo';
import { MotivoDestino } from './modelo/motivo-destino.model';
import { ConsultaMotivoDestino } from './modelo/consulta-motivo';
import { Pagina, PaginaUtils } from '../../shared/paginacion/pagina-utils';
import { HttpUtils } from '../../shared/http-utils';
import { BajaMotivoDestinoComando } from './modelo/comando-baja-motivo-destino';

@Injectable()

export class MotivoDestinoService {
  private url = '/motivosdestino';

  private static filtrosConsulta: ConsultaMotivoDestino;

  constructor(private http: Http,
              private errorHandler: ErrorHandler) {
  }

  public static guardarFiltros(filtros: ConsultaMotivoDestino): void {
    this.filtrosConsulta = filtros;
  }

  public static recuperarFiltros(): ConsultaMotivoDestino {
    return this.filtrosConsulta;
  }

  public consultarMotivosDestino(): Observable<MotivoDestino[]> {
    return this.http.get(this.url)
      .map((res) => MapUtils.extractModel(MotivoDestino, res))
      .catch(this.errorHandler.handle);
  }

  public consultarMotivosDestinoPaginados(consulta: ConsultaMotivoDestino): Observable<Pagina<MotivoDestino[]>> {
    return this.http.get(this.url + '/consultar', {search: HttpUtils.insertarPrefijo(consulta)})
      .map((res) => PaginaUtils.extraerPagina(MotivoDestino, res))
      .catch(this.errorHandler.handle);
  }

  public registrarMotivoDestino(motivo: MotivoDestino): Observable<MotivoDestino> {
    return this.http.post(this.url, motivo)
      .map((res) => MapUtils.extractModel(MotivoDestino, res))
      .catch(this.errorHandler.handle);
  }

  public editarMotivoDestino(motivo: MotivoDestino): Observable<any> {
    return this.http.put(this.url + `/${motivo.id}`, motivo)
      .map((res) => res.json())
      .catch(this.errorHandler.handle);
  }

  public consultarMotivoDestino(id: number): Observable<MotivoDestino> {
    return this.http.get(this.url + `/${id}`)
      .map((res) => MapUtils.extractModel(MotivoDestino, res))
      .catch(this.errorHandler.handle);
  }

  public darDeBajaArea(id: number, comando: BajaMotivoDestinoComando): Observable<any> {
    let parametrosComando = MotivoDestinoService.setParametrosComando(comando);
    return this.http.delete(`${this.url}/${id}`, {params: parametrosComando})
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
