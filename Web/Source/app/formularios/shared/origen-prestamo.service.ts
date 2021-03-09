import { Injectable } from "@angular/core";
import { ErrorHandler } from "../../core/http/error-handler.service";
import { Http, Response } from "@angular/http";
import { Observable } from "rxjs";
import { MapUtils } from "../../shared/map-utils";
import { OrigenPrestamo } from "./modelo/origen-prestamo.model";

@Injectable()

export class OrigenService {
  private url = '/origenesformulario';

  constructor(private http: Http,
              private errorHandler: ErrorHandler) {
  }

  public consultarOrigenes(): Observable<OrigenPrestamo[]> {
    return this.http.get(this.url)
      .map(OrigenService.extraerOrigenes)
      .catch(this.errorHandler.handle);
  }

  private static extraerOrigenes(res: Response): OrigenPrestamo[] {
    let resultado = res.json().resultado;
    return (resultado || []).map((origen) => MapUtils.deserialize(OrigenPrestamo, origen));
  }
}
