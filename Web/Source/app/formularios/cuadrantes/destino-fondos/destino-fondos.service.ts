import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs';
import { ErrorHandler } from '../../../core/http/error-handler.service';
import { DestinoFondos } from '../../../shared/modelo/destino-fondos.model';
import { MapUtils } from '../../../shared/map-utils';

@Injectable()

export class DestinoFondosService {
  private url = '/destinosfondos';

  constructor(private http: Http,
              private errorHandler: ErrorHandler) {
  }

  public consultarDestinosFondos(): Observable<DestinoFondos[]> {
    return this.http.get(this.url)
      .map(DestinoFondosService.extraerDestinosFondos)
      .catch(this.errorHandler.handle);
  }

  private static extraerDestinosFondos(res: Response): DestinoFondos[] {
    let resultado = res.json().resultado;
    return (resultado || []).map((destino) => MapUtils.deserialize(DestinoFondos, destino));
  }
}
