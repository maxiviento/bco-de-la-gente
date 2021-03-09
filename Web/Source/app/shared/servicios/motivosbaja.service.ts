import { Observable } from 'rxjs';
import { ErrorHandler} from '../../core/http/error-handler.service';
import { List } from 'lodash';
import { MotivoBaja } from '../modelo/motivoBaja.model';
import { MapUtils } from '../map-utils';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import 'rxjs/add/observable/throw';
import { Http, Response } from '@angular/http';
import { Injectable } from '@angular/core';

@Injectable()

export class MotivosBajaService {
  public url: string = '/MotivosBaja';

  constructor(private http: Http, private errorHandler: ErrorHandler) {
  }

  public consultarMotivosBaja(): Observable<List<MotivoBaja>> {
    return this.http.get(this.url)
      .map(MotivosBajaService.extraerMotivosBaja)
      .catch(this.errorHandler.handle);
  }
  private static extraerMotivosBaja(res: Response): MotivoBaja {
    return (res.json().resultado || [])
      .map((motivoBaja) => MapUtils.deserialize(MotivoBaja, motivoBaja));
  }
}
