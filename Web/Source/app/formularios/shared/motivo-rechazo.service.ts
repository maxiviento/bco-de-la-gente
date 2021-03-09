import { Observable } from 'rxjs';
import { ErrorHandler } from '../../core/http/error-handler.service';
import { MapUtils } from '../../shared/map-utils';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import 'rxjs/add/observable/throw';
import { Http } from '@angular/http';
import { Injectable } from '@angular/core';
import { MotivoRechazo } from './modelo/motivo-rechazo';

@Injectable()

export class MotivoRechazoService {
  public url: string = '/motivosRechazo';

  constructor(private http: Http, private errorHandler: ErrorHandler) {
  }

  public consultarMotivoRechazo(ambito: string): Observable<MotivoRechazo []> {
    return this.http.get(this.url + '/' + ambito)
      .map((res) => MapUtils.extractModel(MotivoRechazo, res))
      .catch(this.errorHandler.handle);
  }
}
