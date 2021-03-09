import {Injectable} from '@angular/core';
import {Http, Response, URLSearchParams} from '@angular/http';
import {Observable} from 'rxjs';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import {ErrorHandler} from '../../core/http/error-handler.service';
import {Integrante} from "../../shared/modelo/integrante.model";
import {NgbModal, NgbModalRef} from "@ng-bootstrap/ng-bootstrap";
import {ModalModificarGrupoIntegranteComponent} from "../modal-modificar-grupo-integrante/modal-modificar-grupo-integrante.component";
import {ConsultarGrupoFamiliarIntegrantes} from "./modelo/consultar-grupo-familiar-integrantes.model";

@Injectable()
export class GrupoFamiliarService {

  public url: string = '/GruposFamiliaresExtension';

  constructor(private http: Http,
              private errorHandler: ErrorHandler) {
  }

  public consultar(filtros: any): Observable<any> {
    let params = new URLSearchParams();
    params.set('pais', filtros.pais);
    params.set('dni', filtros.dni);
    params.set('sexo', filtros.sexo);
    return this.http.get(this.url + '/consulta-ingresos-grupo', {search: params})
      .map(this.extraer)
      .catch(this.errorHandler.handle);
  }

  public consultarExistenciaGrupo(filtros: any): Observable<any> {
    let params = new URLSearchParams();
    params.set('pais', filtros.pais);
    params.set('dni', filtros.dni);
    params.set('sexo', filtros.sexo);
    return this.http.get(this.url + '/consulta-existencia-grupo', {search: params})
      .map((res) => {
        return res.json().resultado;
      }).catch(this.errorHandler.handle);
  }

  public consultarGrupoFamiliarIntegrantes(integrantes: ConsultarGrupoFamiliarIntegrantes): Observable<any> {
    return this.http
      .post(this.url + '/consulta-existencia-grupo-integrantes', integrantes)
      .map((res) => {
        return res.json().resultado;
      }).catch(this.errorHandler.handle);
  }

  private extraer(res: Response | any): any {
    let resultado = res.json().resultado;
    if (resultado) {
      return resultado;
    } else {
      throw new Error('El grupo familiar no existe.');
    }
  }

  public obtenerIdGrupoUnico(filtros: any): Observable<any> {
    let params = new URLSearchParams();
    params.set('pais', filtros.pais);
    params.set('dni', filtros.dni);
    params.set('sexo', filtros.sexo);
    return this.http.get(this.url + '/obtener-id-grupo', {search: params})
      .map((res) => {
        return res.json().resultado;
      }).catch(this.errorHandler.handle);
  }
}
