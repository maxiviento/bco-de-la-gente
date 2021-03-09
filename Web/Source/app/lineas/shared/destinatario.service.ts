import { Injectable } from "@angular/core";
import { ErrorHandler } from "../../core/http/error-handler.service";
import { Http, Response } from "@angular/http";
import { Observable } from "rxjs";
import { MapUtils } from "../../shared/map-utils";
import { OrigenPrestamo } from "./modelo/origen-prestamo.modelo";
import { SexoDestinatario } from './modelo/destinatario-prestamo.model';

@Injectable()

export class DestinatarioService {
  private url = '/destinatarios';

  constructor(private http: Http,
              private errorHandler: ErrorHandler) {
  }

  public consultarDestinatarios(): Observable<SexoDestinatario[]> {
    return this.http.get(this.url)
      .map(DestinatarioService.extraerDetinatarios)
      .catch(this.errorHandler.handle);
  }

  private static extraerDetinatarios(res: Response): SexoDestinatario[] {
    let resultado = res.json().resultado;
    return (resultado || []).map((destinatario) => MapUtils.deserialize(SexoDestinatario, destinatario));
  }
}
