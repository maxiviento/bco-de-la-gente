import { Injectable } from '@angular/core';
import { ErrorHandler } from '../../core/http/error-handler.service';
import { ItemConfiguracionChecklist } from './modelo/item-configuracion-checklist.model';
import { Observable } from 'rxjs';
import { MapUtils } from '../../shared/map-utils';
import { ConsultaConfiguracionChecklist } from './modelo/consulta-configuracion-checklist.modelo';
import { Http } from '@angular/http';
import { HttpUtils } from '../../shared/http-utils';
import { EtapaEstadosLineas } from './modelo/etapa-estados-lineas.model';
import { PatrimonioSolicitante } from '../../formularios/shared/modelo/patrimonio-solicitante.model';
import { EtapaEstadoLinea } from '../../prestamos-checklists/shared/modelos/etapa-estado-linea.model';
import { VersionChecklist } from './modelo/version-checklist.model';

@Injectable()
export class ConfiguracionChecklistService {
  private url: string = '/lineasprestamo';

  constructor(private http: Http,
              private errorHandler: ErrorHandler) {
  }

  public consultarItems(consulta: any): Observable<ItemConfiguracionChecklist[]> {
    return this.http.get(this.url + '/requisitos-linea', {search: HttpUtils.insertarPrefijo(consulta)})
      .map((res) => MapUtils.extractModel(ItemConfiguracionChecklist, res))
      .catch(this.errorHandler.handle);
  }

  public registrarConfiguracion(requisitos: ItemConfiguracionChecklist[], idLinea: number, idEtapa: number, etapasEstados: EtapaEstadoLinea[], idsEtapasEliminadas: number[]): Observable<ItemConfiguracionChecklist []> {
    return this.http.post(this.url + '/registrar-configuracion', {requisitos, idLinea, idEtapa, etapasEstados, idsEtapasEliminadas})
      .map((res) => MapUtils.extractModel(ItemConfiguracionChecklist, res))
      .catch(this.errorHandler.handle);
  }

  public consultarEtapasEstadosLinea(idLinea: number, idPrestamo?: number): Observable<EtapaEstadoLinea[]> {
    return this.http
      .get(this.url + `/etapas-estados-linea?idLinea=${idLinea}&idPrestamo=${idPrestamo}`)
      .map((res) => res.json().resultado)
      .catch(this.errorHandler.handle);
  }

  public consultarVersionChecklistVigente(idLinea: number): Observable<VersionChecklist> {
    return this.http
      .get(this.url + `/version-checklist?idLinea=${idLinea}`)
      .map((res) => MapUtils.extractModel(VersionChecklist, res))
      .catch(this.errorHandler.handle);
  }
}
