import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { ErrorHandler } from '../../core/http/error-handler.service';
import { SeleccionMultiple } from './seleccion-multiple.model';

@Injectable()
export class MultipleSeleccionService {
  constructor(private http: Http, private errorHandler: ErrorHandler) {
  }
  private urlLinea = '/lineasprestamo';
  private urlEstadoFormulario = '/estadosformulario';
  private urlEstadoPrestamo = '/prestamos';
  private urlMonitorProceso = '/monitorprocesos';

  public consultarDepartamentos() {
    return this.http
      .get('/Departamentos')
      .map((res: any) => {
        let a = [];
        res.json().resultado.forEach((e) => {
          a.push({
            id: +e.clave,
            name: e.valor
          });
        });
        return a;
      })
      .catch(this.errorHandler.handle);
  }
  public consultarEstadosFormulario() {
    return this.http.get(this.urlEstadoFormulario)
      .map((res: any) => {
        let a = [];
        res.json().resultado.forEach((e) => {
          a.push({
            id: +e.clave,
            name: e.valor
          });
        });
        return a;
      })
      .catch(this.errorHandler.handle);
  }

  public consultarEstadosPrestamo() {
    return this.http.get(this.urlEstadoPrestamo + '/estados-prestamo')
      .map((res: any) => {
        let a = [];
        res.json().resultado.forEach((e) => {
          a.push({
            id: +e.clave,
            name: e.valor
          });
        });
        return a;
      })
      .catch(this.errorHandler.handle);
  }
  public obtenerEstadosProceso() {
    return this.http.get(this.urlMonitorProceso + '/obtener-estados')
      .map((res: any) => {
        let a = [];
        res.json().resultado.forEach((e) => {
          a.push({
            id: +e.clave,
            name: e.valor
          });
        });
        return a;
      })
      .catch(this.errorHandler.handle);
  }
  public obtenerTiposProceso() {
    return this.http.get(this.urlMonitorProceso + '/obtener-tipos')
      .map((res: any) => {
        let a = [];
        res.json().resultado.forEach((e) => {
          a.push({
            id: +e.clave,
            name: e.valor
          });
        });
        return a;
      })
      .catch(this.errorHandler.handle);
  }

  public consultarLineas() {
    return this.http.get(this.urlLinea + '/GetLineasParaCombo' + '?multiple=' + !!true)
      .map((res: any) => {
        let a = [];
        res.json().resultado.forEach((e) => {
          a.push({
            id: +e.id,
            name: e.descripcion,
            dadoDeBaja: e.dadoDeBaja
          });
        });
        return a;
      })
      .catch(this.errorHandler.handle);
  }

  public consultarLocalidades(depto: SeleccionMultiple) {
    return this.http.get(`/localidades/${depto.id}`)
      .map((res: any) => {
        let a = [];
        a.push({
          id: -depto.id,
          name: depto.name,
          isLabel: true
        });
        res.json().resultado.forEach((e) => {
          a.push({
            id: +e.clave,
            name: e.valor,
            parentId: -depto.id
          });
        });
        return a;
      })
      .catch(this.errorHandler.handle);
  }

}
