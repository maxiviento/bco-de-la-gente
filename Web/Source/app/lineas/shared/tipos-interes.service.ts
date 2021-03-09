import { Injectable } from "@angular/core";
import { ErrorHandler } from "../../core/http/error-handler.service";
import { Http, Response } from "@angular/http";
import { Observable } from "rxjs";
import { MapUtils } from "../../shared/map-utils";
import { OrigenPrestamo } from "./modelo/origen-prestamo.modelo";
import { TipoInteres } from './modelo/tipo-interes.model';

@Injectable()

export class TiposInteresService {
  private url = '/tiposinteres';

  constructor(private http: Http,
              private errorHandler: ErrorHandler) {
  }

  public consultarTiposInteres(): Observable<TipoInteres[]> {
    return this.http.get(this.url)
      .map(TiposInteresService.extraerTiposInteres)
      .catch(this.errorHandler.handle);
  }

  private static extraerTiposInteres(res: Response): TipoInteres[] {
    let resultado = res.json().resultado;
    return (resultado || []).map((tipoInteres) => MapUtils.deserialize(TipoInteres, tipoInteres));
  }
}
