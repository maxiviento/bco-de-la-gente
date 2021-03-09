import { Injectable } from "@angular/core";
import { ErrorHandler } from "../../core/http/error-handler.service";
import { Http, Response } from "@angular/http";
import { Observable } from "rxjs";
import { MapUtils } from "../../shared/map-utils";
import { OrigenPrestamo } from "./modelo/origen-prestamo.modelo";
import { TipoFinanciamiento } from './modelo/tipo-financiamiento.model';

@Injectable()

export class TiposFinanciamientoService {
  private url = '/tiposfinanciamiento';

  constructor(private http: Http,
              private errorHandler: ErrorHandler) {
  }

  public consultarTiposFinanciamiento(): Observable<TipoFinanciamiento[]> {
    return this.http.get(this.url)
      .map(TiposFinanciamientoService.extraerTiposFinanciamiento)
      .catch(this.errorHandler.handle);
  }

  private static extraerTiposFinanciamiento(res: Response): TipoFinanciamiento[] {
    let resultado = res.json().resultado;
    return (resultado || []).map((tiposFinanciamiento) => MapUtils.deserialize(TipoFinanciamiento, tiposFinanciamiento));
  }
}
