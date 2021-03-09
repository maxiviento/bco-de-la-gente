import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { ErrorHandler } from '../../core/http/error-handler.service';
import { SeleccionMultiple } from './seleccion-multiple.model';

@Injectable()
export class FiltroDomicilioBandejaService {
  constructor(private http: Http, private errorHandler: ErrorHandler) {
  }

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
