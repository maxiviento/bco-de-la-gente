import {Injectable} from '@angular/core';
import {MapUtils} from '../map-utils';
import {Departamento} from '../../formularios/shared/modelo/departamento.model';
import {Http, Response} from '@angular/http';
import {ErrorHandler} from '../../core/http/error-handler.service';
import {Observable} from "rxjs";
import {Localidad} from "../../formularios/shared/modelo/localidad.model";
import {SeleccionMultiple} from "./seleccion-multiple.model";

@Injectable()
export class FiltroDomicilioService {
  constructor(private http: Http, private errorHandler: ErrorHandler) {
  }

  public consultarDepartamentos() {
    return this.http
      .get('/Departamentos')
      .map((res: any) => {
        let a = [];
        res.json().resultado.forEach(e => {
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
        res.json().resultado.forEach(e => {
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
