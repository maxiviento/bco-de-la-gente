import { Injectable } from '@angular/core';
import { ErrorHandler } from '../../core/http/error-handler.service';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs';
import { MapUtils } from '../../shared/map-utils';
import { EstadoFormulario } from './modelo/estado-formulario.model';

@Injectable()

export class EstadoFormularioService {
  private url = '/estadosformulario';

  constructor(private http: Http,
              private errorHandler: ErrorHandler) {
  }

  public consultarEstadosFormulario(): Observable<EstadoFormulario[]> {
    return this.http.get(this.url)
      .map(EstadoFormularioService.extraerEstadosFormulario)
      .catch(this.errorHandler.handle);
  }

  public consultarEstadosParaPrestamos(): Observable<EstadoFormulario[]> {
    return this.http.get(this.url + '/prestamos')
      .map(EstadoFormularioService.extraerEstadosFormulario)
      .catch(this.errorHandler.handle);
  }

  private static extraerEstadosFormulario(res: Response): EstadoFormulario[] {
    let resultado = res.json().resultado;
    return (resultado || []).map((estado) => MapUtils.deserialize(EstadoFormulario, estado));
  }

  public consultarEstadosFiltroComboCambioEstado(): Observable<EstadoFormulario[]> {
    return this.http.get(this.url + '/consultar-estados-filtro-cambio-estado')
      .map(EstadoFormularioService.extraerEstadosFormulario)
      .catch(this.errorHandler.handle);
  }
}
