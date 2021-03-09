import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import { ErrorHandler } from '../../core/http/error-handler.service';
import { Sexo } from '../modelo/sexo.model';
import { MapUtils } from '../map-utils';

@Injectable()
export class SexoService {
  private url = '/sexos';

  constructor(private http: Http,
              private errorHandler: ErrorHandler) {
  }

  public obtenerSexos(): Observable<Sexo[]> {
    return this.http
      .get(this.url)
      .map(this.extraer)
      .catch(this.errorHandler.handle);
  }

  private extraer(res: Response): Sexo [] {
    return (res.json().resultado || [])
      .map((estados) => MapUtils.deserialize(Sexo, estados));
  }
}
