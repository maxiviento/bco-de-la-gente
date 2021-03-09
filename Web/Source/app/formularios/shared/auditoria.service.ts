import { Injectable } from '@angular/core';
import { ErrorHandler } from '../../core/http/error-handler.service';
import { Http } from '@angular/http';
import { Observable } from 'rxjs';
import { Auditoria } from '../../shared/modelo/auditoria.modelo';

@Injectable()
export class AuditoriaService {
  private url = '/Formularios';

  constructor(private http: Http,
              private errorHandler: ErrorHandler) {
  }

  public registrarSeguimiento(auditoria: Auditoria): Observable<any> {
    return this.http
      .post(`${this.url}/registrar-seguimiento-auditoria`, auditoria)
      .catch(this.errorHandler.handle);
  }
}
