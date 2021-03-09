import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import { ErrorHandler } from '../../core/http/error-handler.service';
import { MapUtils } from '../map-utils';
import { Pais } from '../modelo/pais.model';

@Injectable()
export class PaisService {
  private url = '/paises';

  constructor(private http: Http,
              private errorHandler: ErrorHandler) {
  }

  public obtenerPaises(): Observable<Pais[]> {
    return this.http
      .get(this.url)
      .map(this.extraer)
      .catch(this.errorHandler.handle);
  }

  private extraer(res: Response): Pais [] {
    return (res.json().resultado || [])
      .map((estados) => MapUtils.deserialize(Pais, estados));
  }
}
