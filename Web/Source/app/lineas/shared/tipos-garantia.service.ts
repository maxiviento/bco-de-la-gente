import { Injectable } from "@angular/core";
import { ErrorHandler } from "../../core/http/error-handler.service";
import { Http, Response } from "@angular/http";
import { Observable } from "rxjs";
import { MapUtils } from "../../shared/map-utils";
import { OrigenPrestamo } from "./modelo/origen-prestamo.modelo";
import { TipoGarantia } from './modelo/tipo-garantia.model';

@Injectable()

export class TiposGarantiaService {
  private url = '/tiposgarantia';

  constructor(private http: Http,
              private errorHandler: ErrorHandler) {
  }

  public consultarTiposGarantia(): Observable<TipoGarantia[]> {
    return this.http.get(this.url)
      .map(TiposGarantiaService.extraerTiposGarantia)
      .catch(this.errorHandler.handle);
  }

  private static extraerTiposGarantia(res: Response): TipoGarantia[] {
    let resultado = res.json().resultado;
    return (resultado || []).map((tipoGarantia) => MapUtils.deserialize(TipoGarantia, tipoGarantia));
  }
}
