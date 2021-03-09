import { Injectable } from '@angular/core';
import { ErrorHandler } from '../../core/http/error-handler.service';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs';
import { Localidad } from './modelo/localidad.model';
import { MapUtils } from '../../shared/map-utils';

@Injectable()

export class LocalidadComboServicio {
  private url = '/Localidades';

  constructor(private http: Http,
              private errorHandler: ErrorHandler) {
  }

  public consultarLocalidades(idDepartamento?: number): Observable<Localidad[]> {
    return this.http.get(`${this.url}/${idDepartamento}`)
      .map((res) => MapUtils.extractModel(Localidad, res))
      .catch(this.errorHandler.handle);
  }
}
