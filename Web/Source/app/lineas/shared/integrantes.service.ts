import { Injectable } from "@angular/core";
import { ErrorHandler } from "../../core/http/error-handler.service";
import { Http, Response } from "@angular/http";
import { Observable } from "rxjs";
import { MapUtils } from "../../shared/map-utils";
import { OrigenPrestamo } from "./modelo/origen-prestamo.modelo";
import { IntegranteSocio } from './modelo/integrante-socio.model';

@Injectable()

export class IntegrantesService {
  private url = '/integrantes';

  constructor(private http: Http,
              private errorHandler: ErrorHandler) {
  }

  public consultarIntegrantes(): Observable<IntegranteSocio[]> {
    return this.http.get(this.url)
      .map(IntegrantesService.extraerIntegrantes)
      .catch(this.errorHandler.handle);
  }

  private static extraerIntegrantes(res: Response): IntegranteSocio[] {
    let resultado = res.json().resultado;
    return (resultado || []).map((integrantes) => MapUtils.deserialize(IntegranteSocio, integrantes));
  }
}
