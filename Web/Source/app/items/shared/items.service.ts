import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs';
import { Item } from './modelo/item.model';
import { MapUtils } from '../../shared/map-utils';
import { ConsultaItem } from './modelo/consulta-item.model';
import { ErrorHandler } from '../../core/http/error-handler.service';
import { BajaItemComando } from './modelo/comando-baja-item.model';
import { Pagina, PaginaUtils } from '../../shared/paginacion/pagina-utils';
import { HttpUtils } from '../../shared/http-utils';
import { TipoItem } from './modelo/tipo-item.model';
import { ItemTipoItem } from './modelo/item-tipo-item';
import { Recurso } from './modelo/recurso.model';

@Injectable()
export class ItemsService {
  private url = '/items';

  private static filtrosConsulta: ConsultaItem;

  constructor(private http: Http,
              private errorHandler: ErrorHandler) {
  }

  public static guardarFiltros(filtros: ConsultaItem): void {
    this.filtrosConsulta = filtros;
  }

  public static recuperarFiltros(): ConsultaItem {
    return this.filtrosConsulta;
  }

  public registrarItem(item: Item): Observable<Item> {
    return this.http.post(this.url, item)
      .map(ItemsService.extraerItem)
      .catch(this.errorHandler.handle);
  }

  public consultarTiposItem(): Observable<TipoItem []> {
    return this.http.get(this.url + '/tiposItem')
      .map(ItemsService.extraerTiposItem)
      .catch(this.errorHandler.handle);
  }

  public consultarItemsPorTipoItem(esCreacionDeLinea: boolean): Observable<Item[]> {
    return this.http.get(this.url + '/itemsPorTipoItem' + '?esCreacionDeLinea=' + !!esCreacionDeLinea)
      .map(ItemsService.extraerItems)
      .catch(this.errorHandler.handle);
  }

  public editarItem(item: Item): Observable<void> {
    return this.http.put(`${this.url}/${item.id}`, item)
      .map((response) => response.json())
      .catch(this.errorHandler.handle);
  }

  public consultarItems(consultaItem: ConsultaItem): Observable<Pagina<Item>> {
    return this.http
      .get(this.url + '/consultar', {search: HttpUtils.insertarPrefijo(consultaItem)})
      .map((res) => {
        return PaginaUtils.extraerPagina(Item, res);
      })
      .catch(this.errorHandler.handle);
  }

  public consultarItemsCombo(): Observable<Item []> {
    return this.http.get(this.url + '/consultar-items-combo')
      .map(ItemsService.extraerItems)
      .catch(this.errorHandler.handle);
  }

  public consultarItemsPadre(): Observable<Item []> {
    return this.http.get(this.url + '/consultar-items')
      .map(ItemsService.extraerItems)
      .catch(this.errorHandler.handle);
  }

  public consultarRecursos(): Observable<Recurso []> {
    return this.http.get(this.url + '/consultar-recursos')
      .map(ItemsService.extraerRecursos)
      .catch(this.errorHandler.handle);
  }

  private static extraerItem(res: Response): Item {

    let resultado = res.json().resultado;
    return resultado ? MapUtils.deserialize(Item, resultado) : undefined;
  }

  private static extraerRecursos(res: Response): Recurso [] {
    let resultado = res.json().resultado;
    return (resultado || []).map((recurso) => MapUtils.deserialize(Recurso, recurso));
  }

  private static extraerItems(res: Response): Item [] {
    let resultado = res.json().resultado;
    return (resultado || []).map((item) => MapUtils.deserialize(Item, item));
  }

  private static extraerItemDetalle(res: Response): Item {
    let resultado = res.json().resultado;
    return resultado ? MapUtils.deserialize(Item, resultado) : undefined;
  }

  public consultarItemPorId(id: number): Observable<Item> {
    return this.http
      .get(`${this.url}/${id}`)
      .map(ItemsService.extraerItemDetalle)
      .catch(this.errorHandler.handle);
  }

  public darDeBajaItem(idItem: number, comando: BajaItemComando): Observable<any> {
    let parametrosComando = ItemsService.setParametrosComando(comando);

    return this.http.delete(`${this.url}/${idItem}`, {params: parametrosComando})
      .map((response) => response.json())
      .catch(this.errorHandler.handle);
  }

  private static setParametrosComando(comando: any): object {
    let params = {};
    Object.getOwnPropertyNames(comando)
      .map((key: string) => {
        params['comando.' + key] = comando[key];
      });
    return params;
  }

  public static extraerTiposItem(res: Response): TipoItem [] {
    let resultado = res.json().resultado;
    return (resultado || []).map((tipoItem) => MapUtils.deserialize(TipoItem, tipoItem));
  }

  public static extraerItemsPorTipoItem(res: Response): ItemTipoItem [] {
    let resultado = res.json().resultado;
    return (resultado || []).map((value) => MapUtils.deserialize(ItemTipoItem, value));
  }

  public poseeHijos(idItem: number): Observable<boolean> {
    return this.http.get(`${this.url}/posee-hijos//${idItem}`)
      .map((res) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }
}
