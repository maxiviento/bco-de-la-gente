import { Injectable } from '@angular/core';
import { Http, Response, URLSearchParams } from '@angular/http';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import { ErrorHandler } from '../../../core/http/error-handler.service';
import { Persona } from '../../../shared/modelo/persona.model';

@Injectable()
export class PersonaService {

  public url: string = '/personas';

  constructor(private http: Http, private errorHandler: ErrorHandler) {
  }

  public consultarPersona(filtros: Persona): Observable<Persona> {
    let params = new URLSearchParams();
    params.set('pais', filtros.codigoPais);
    params.set('dni', filtros.nroDocumento);
    // agrego padding con 0 solo cuando no lo tiene
    let sexoParam = filtros.sexoId.length === 1 ? '0' + filtros.sexoId : filtros.sexoId;
    params.set('sexo', sexoParam);
    return this.http.get(this.url + '/consulta-datos-completo', {search: params})
      .map(this.extraerPersona)
      .catch(this.errorHandler.handle);
  }

  private extraerPersona(res: Response | any): Persona {
    let resultado = res.json().resultado;
    if (resultado) {
      return resultado;
    } else {
      throw new Error('La persona no existe.');
    }
  }
}
