import { Injectable } from '@angular/core';
import { ErrorHandler } from '../../core/http/error-handler.service';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { MapUtils } from '../map-utils';
import { ItemCombo } from '../modelo/item-combo.model';

@Injectable()

export class BancoService {
  private url = '/Bancos';

  constructor(private http: Http,
              private errorHandler: ErrorHandler) {
  }

  public consultarBancos(): Observable<ItemCombo[]> {
    return this.http.get(this.url + '/obtener-combo-bancos')
      .map(BancoService.extraerItemsCombo)
      .catch(this.errorHandler.handle);
  }

  public consultarSucursales(idBanco: string): Observable<ItemCombo[]> {
    return this.http.get(`${this.url}/obtener-combo-sucursales/${idBanco}`)
      .map(BancoService.extraerItemsCombo)
      .catch(this.errorHandler.handle);
  }

  private static extraerItemsCombo(res: Response): ItemCombo[] {
    let resultado = res.json().resultado;
    return (resultado || []).map((localidad) => MapUtils.deserialize(ItemCombo, localidad));
  }
}
