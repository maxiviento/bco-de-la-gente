import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';

import { Observable } from 'rxjs/Observable';

import AccionGrupoUnico from './accion-grupo-unico.enum';
import { ErrorHandler } from '../../core/http/error-handler.service';

@Injectable()
export class GrupoUnicoService {
  constructor(private http: Http, private errorHandler: ErrorHandler) {
  }

  public obtenerUrlAutorizada(recurso: AccionGrupoUnico,
                              dni: string,
                              sexo: string,
                              pais: string): Observable<string> {

    return this.http.get(`${recurso}?sexo=${sexo}&dni=${dni}&pais=${pais}`)
      .map((response: Response) => response.json().resultado)
      .catch(this.errorHandler.handle);
  }
}
