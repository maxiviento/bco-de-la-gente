import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import { ErrorHandler } from '../../core/http/error-handler.service';
import { MapUtils } from '../map-utils';
import { TipoDocumentacion } from '../modelo/tipo.documentacion.model';

@Injectable()
export class TipoDocumentacionService {
  private url = '/documentaciones';

  constructor(private http: Http,
              private errorHandler: ErrorHandler) {
  }

  public obtenerTiposDocumentacion(): Observable<TipoDocumentacion[]> {
    return this.http
      .get(this.url + '/tipos-documentacion')
      .map((res) => MapUtils.extractModel(TipoDocumentacion, res))
      .catch(this.errorHandler.handle);
  }
}
