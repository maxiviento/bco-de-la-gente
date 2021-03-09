import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { ErrorHandler } from '../../core/http/error-handler.service';
import AccionGrupoUnico from './accion-grupo-unico.enum';

@Injectable()
export class DomicilioService {
  constructor(private http: Http, private errorHandler: ErrorHandler) {
  }

  public consultarDomicilio(recurso: AccionGrupoUnico, dni: string, sexo: string, pais: string): Observable<any> {
    let tipoDomicilio = 3;
    return this.http.get(`${recurso}?sexo=${sexo}&dni=${dni}&pais=${pais}&tipoDomicilio=${tipoDomicilio}`)
      .map((response: Response) => response.json().resultado)
      .catch(this.errorHandler.handle);
  }

  public consultarDomicilioPorIdVin(recurso: AccionGrupoUnico, idVin: number): Observable<any> {
    return this.http.get(`${recurso}?idVin=${idVin}`)
      .map((response: Response) => response.json().resultado)
      .catch(this.errorHandler.handle);
  }
}
