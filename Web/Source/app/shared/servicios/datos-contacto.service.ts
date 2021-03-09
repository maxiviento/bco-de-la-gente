import { Injectable } from '@angular/core';
import { ErrorHandler } from '../../core/http/error-handler.service';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { MapUtils } from '../map-utils';
import { Persona } from '../modelo/persona.model';
import { HttpUtils } from '../http-utils';
import { DatosContacto } from '../modelo/datos-contacto.model';
import { ObtenerDatosContactoConsulta } from '../modelo/consultas/obtener-datos-contacto-consulta.model';
import { ActualizarDatosDeContactoComando } from '../../formularios/shared/modelo/actualizar-datos-contacto-comando.model';

@Injectable()

export class ContactoService {
  private url = '/Formularios';

  constructor(private http: Http,
              private errorHandler: ErrorHandler) {
  }

  public obtenerDatosDeContacto(persona: Persona): Observable<DatosContacto> {
    let consulta = this.armarConsultaDatosContacto(persona);
    return this.http.get(this.url + '/consulta-datos-contacto', {search: HttpUtils.insertarPrefijo(consulta)})
      .map(ContactoService.extraerDatosContacto)
      .catch(this.errorHandler.handle);
  }

  private static extraerDatosContacto(res: Response | any): DatosContacto {
    let resultado = res.json().resultado;

    if (resultado) {
      return MapUtils.deserialize(DatosContacto, resultado);
    }
  }

  private armarConsultaDatosContacto(persona: Persona): ObtenerDatosContactoConsulta {
    return new ObtenerDatosContactoConsulta(
      persona.sexoId,
      persona.nroDocumento,
      persona.codigoPais,
      persona.idNumero.toString());
  }

  public actualizarDatosDeContacto(comando: ActualizarDatosDeContactoComando): Observable<any> {
    return this.http
      .post(`${this.url}/actualizar-datos-contacto`, comando)
      .map((response) => response.json().resultado)
      .catch(this.errorHandler.handle);
  }
}
