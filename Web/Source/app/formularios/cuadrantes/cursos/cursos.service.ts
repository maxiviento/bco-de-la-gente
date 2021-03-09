import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import { Curso } from '../../shared/modelo/curso.model';
import { MapUtils } from '../../../shared/map-utils';
import { ErrorHandler } from '../../../core/http/error-handler.service';

@Injectable()
export class CursosService {
  private static extraerCursos(response: Response): Curso[] {
    let resultado = response.json().resultado;
    if (resultado) {
      return (resultado || []).map((curso) => MapUtils.deserialize(Curso, curso));
    }
  }

  public url: string = '/Cursos';

  constructor(private http: Http, private errorHandler: ErrorHandler) {
  }

  public consultarCursos(): Observable<Curso[]> {
    return this.http
      .get(this.url)
      .map(CursosService.extraerCursos)
      .catch(this.errorHandler.handle);
  }
}
