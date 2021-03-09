import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Http, Response } from '@angular/http';
import { ErrorHandler } from '../core/http/error-handler.service';
import { MapUtils } from '../shared/map-utils';
import { Archivo } from '../shared/modelo/archivo.model';
import { ConsultaManual } from './modelo/consulta-manual.model';
import { HttpUtils } from '../shared/http-utils';

@Injectable()
export class ManualesService {
  private url = '/manuales';

  constructor(private http: Http,
              private errorHandler: ErrorHandler) {
  }

  public obtenerManuales(): Observable<string[]> {
    return this.http.get(this.url)
      .map((res: Response) => {
        return res.json().resultado;
      }).catch(this.errorHandler.handle);
  }

  public descargarManual(consulta: ConsultaManual): Observable<Archivo>{
    return this.http.get(this.url + '/descargar/', {search: HttpUtils.insertarPrefijo(consulta)})
      .map((res) => MapUtils.extractModel(Archivo, res))
      .catch(this.errorHandler.handle);
  }
}
