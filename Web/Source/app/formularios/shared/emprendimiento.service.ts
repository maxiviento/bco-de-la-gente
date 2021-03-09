import { Injectable } from '@angular/core';
import { ErrorHandler } from '../../core/http/error-handler.service';
import { Http } from '@angular/http';
import { Observable } from 'rxjs';
import { MapUtils } from '../../shared/map-utils';
import { TipoProyecto } from './modelo/tipo-proyecto.model';
import { SectorDesarrollo } from './modelo/sector-desarrollo.model';
import { TipoInmueble } from './modelo/tipo-inmueble.model';
import { Actividad } from './modelo/actividad.model';
import { Rubro } from './modelo/rubro.model';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ModalIntegranteEmprendimientoComponent } from '../modal-integrante-emprendimiento/modal-integrante-emprendimiento.component';
import { MiembroEmprendimiento } from './modelo/miembro-emprendimiento.model';
import { Vinculo } from './modelo/vinculo.model';
import { TipoOrganizacion } from './modelo/tipo-organizacion.model';
import { ItemMercadoComercializacion } from './modelo/item-mercado-comercializacion.model';
import { MercadoComercializacionComando } from './modelo/mercado-comercializacion-comando.model';
import { ComboInstituciones } from './modelo/combo-instituciones.model';
import { TipoCosto } from './modelo/tipo-costo.model';

@Injectable()

export class EmprendimientoService {
  private url = '/emprendimientos';

  constructor(private http: Http,
              private modalService: NgbModal,
              private errorHandler: ErrorHandler) {
  }

  public consultarTiposProyecto(): Observable<TipoProyecto[]> {
    return this.http.get(this.url + '/tipos-proyecto')
      .map((res) => MapUtils.extractModel(TipoProyecto, res))
      .catch(this.errorHandler.handle);
  }

  public consultarTiposInmueble(): Observable<TipoInmueble[]> {
    return this.http.get(this.url + '/tipos-inmueble')
      .map((res) => MapUtils.extractModel(TipoInmueble, res))
      .catch(this.errorHandler.handle);
  }

  public consultarSectoresDesarrollo(): Observable<SectorDesarrollo[]> {
    return this.http.get(this.url + '/sectores-desarrollo')
      .map((res) => MapUtils.extractModel(SectorDesarrollo, res))
      .catch(this.errorHandler.handle);
  }

  public consultarRubros(): Observable<Rubro[]> {
    return this.http.get(this.url + '/rubros')
      .map((res) => MapUtils.extractModel(Rubro, res))
      .catch(this.errorHandler.handle);
  }

  public consultarActividades(idRubro?: number): Observable<Actividad[]> {
    return this.http.get(`${this.url}/actividades/${idRubro}`)
      .map((res) => MapUtils.extractModel(Actividad, res))
      .catch(this.errorHandler.handle);
  }

  public consultarInstituciones(): Observable<ComboInstituciones[]> {
    return this.http.get(this.url + '/instituciones')
      .map((res) => MapUtils.extractModel(ComboInstituciones, res))
      .catch(this.errorHandler.handle);
  }

  public consultarVinculos(): Observable<Vinculo[]> {
    return this.http.get(this.url + '/vinculos')
      .map((res) => MapUtils.extractModel(Vinculo, res))
      .catch(this.errorHandler.handle);
  }

  public consultarTiposOrganizacion(): Observable<TipoOrganizacion[]> {
    return this.http.get(this.url + '/tipos-organizacion')
      .map((res) => MapUtils.extractModel(TipoOrganizacion, res))
      .catch(this.errorHandler.handle);
  }

  public modalMiembroEmprendimiento(vinculos: any, miembro?: MiembroEmprendimiento): NgbModalRef {
    const modalRef = this.modalService.open(ModalIntegranteEmprendimientoComponent, {
      backdrop: 'static',
      windowClass: 'modal-l'
    });
    modalRef.componentInstance.vinculos = vinculos;
    modalRef.componentInstance.miembro = miembro;
    return modalRef;
  }

  public consultarItemsMercadoYComercializacion(): Observable<ItemMercadoComercializacion[]> {
    return this.http
      .get(this.url + '/obtener-items-comercializacion')
      .map((res) => EmprendimientoService.extraerItemsMercadoYComercializacion(res))
      .catch(this.errorHandler.handle);
  }

  private static extraerItemsMercadoYComercializacion(response: any): ItemMercadoComercializacion[] {
    let resultado = response.json().resultado;
    if (resultado) {
      return (resultado || []).map((curso) => MapUtils.deserialize(ItemMercadoComercializacion, curso));
    }
  }

  public guardarMercadoComercializacion(comando: MercadoComercializacionComando): Observable<boolean> {
    return this.http
      .post(this.url + '/guardar-mercado-comercializacion', comando)
      .map((res) => {
        return res;
      })
      .catch(this.errorHandler.handle);
  }

  public consultarItemsPrecioVenta(): Observable<TipoCosto[]> {
    return this.http.get(this.url + '/items-precio-venta')
      .map((res) => MapUtils.extractModel(TipoCosto, res))
      .catch(this.errorHandler.handle);
  }
}
