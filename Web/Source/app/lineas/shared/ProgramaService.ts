import { Injectable } from '@angular/core';
import { ErrorHandler } from '../../core/http/error-handler.service';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs';
import { MapUtils } from '../../shared/map-utils';
import { OrigenPrestamo } from './modelo/origen-prestamo.modelo';
import { ProgramaCombo } from './modelo/programa-combo.model';

@Injectable()

export class ProgramaService {
  private url = '/programas';

  constructor(private http: Http,
              private errorHandler: ErrorHandler) {
  }

  public consultarProgramas(): Observable<ProgramaCombo[]> {
    return this.http.get(this.url)
      .map(ProgramaService.extraerProgramas)
      .catch(this.errorHandler.handle);
  }

  private static extraerProgramas(res: Response): ProgramaCombo[] {
    let resultado = res.json().resultado;
    return (resultado || []).map((programa) => MapUtils.deserialize(ProgramaCombo, programa));
  }
}
