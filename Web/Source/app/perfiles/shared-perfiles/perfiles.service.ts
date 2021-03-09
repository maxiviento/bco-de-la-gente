import { Injectable } from '@angular/core';
import { Pagina, PaginaUtils } from '../../shared/paginacion/pagina-utils';
import { HttpUtils } from '../../shared/http-utils';
import { Http, Response } from '@angular/http';
import { FiltrosPerfiles } from './modelo/filtros-perfiles.model';
import { ItemPerfiles } from './modelo/item-perfiles.model';
import { Observable } from 'rxjs';
import { ErrorHandler } from '../../core/http/error-handler.service';
import { MapUtils } from '../../shared/map-utils';
import { Funcionalidad } from './modelo/funcionalidad.model';
import { Perfil } from './modelo/perfil.model';
import { Motivo } from './modelo/motivo.model';

@Injectable()
export class PerfilesService {
  private static filtrosConsulta: FiltrosPerfiles;
  public url: string = '/perfiles';

  constructor(private http: Http,
              private errorHandler: ErrorHandler) {
  }

  public static guardarFiltros(filtros: FiltrosPerfiles): void {
    this.filtrosConsulta = filtros;
  }

  public static recuperarFiltros(): FiltrosPerfiles {
    return this.filtrosConsulta;
  }

  public consultarPerfiles(filtrosRequisitos: FiltrosPerfiles): Observable<Pagina<ItemPerfiles[]>> {
    return this.http
      .get(this.url, {search: HttpUtils.insertarPrefijo(filtrosRequisitos)})
      .map((res) => {
        return PaginaUtils.extraerPagina(ItemPerfiles, res);
      })
      .catch(this.errorHandler.handle);
  }

  public obtenerFuncionalidades(): Observable<Funcionalidad[]> {
    return this.http
      .get(`${this.url}/funcionalidades`)
      .map(PerfilesService.extraerFuncionalidades)
      .catch(this.errorHandler.handle);
  }

  public obtenerMotivos(): Observable<Motivo[]> {
    return this.http
      .get(`/motivosbaja/${this.url}`)
      .map(PerfilesService.extraerMotivos)
      .catch(this.errorHandler.handle);
  }

  public registrarPerfil(perfil: Perfil): Observable<number> {
    return this.http
      .post(this.url, perfil)
      .map((res) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public eliminarPerfil(perfilId: number, motivoBajaId: string): Observable<void> {
    return this.http
      .put(`${this.url}/${perfilId}/baja`, {motivoBajaId})
      .map((res) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public modificarPerfil(perfilId: number, perfil: Perfil): Observable<void> {
    return this.http
      .put(`${this.url}/${perfilId}`, perfil)
      .map((res) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  private static extraerMotivos(res: Response): Motivo[] {
    return (res.json().resultado || [])
      .map((motivo) => MapUtils.deserialize(Motivo, motivo));
  }

  private static extraerFuncionalidades(res: Response): Funcionalidad[] {
    return (res.json().resultado || [])
      .map((funcionalidad) => MapUtils.deserialize(Funcionalidad, funcionalidad));
  }

  public consultarPerfil(perfilId: number): Observable<Perfil> {

    return this.http
      .get(`${this.url}/${perfilId}`)
      .map(PerfilesService.extraerPerfil)
      .catch(this.errorHandler.handle);
  }

  public consultarFuncionalidadesPerfil(perfilId: number): Observable<Funcionalidad[]> {
    return this.http
      .get(`${this.url}/${perfilId}/funcionalidades`)
      .map(PerfilesService.extraerFuncionalidades)
      .catch(this.errorHandler.handle);
  }

  private static extraerPerfil(res: Response): Perfil {
    let resultado = res.json().resultado;
    if (resultado) {
      return MapUtils.deserialize(Perfil, resultado);
    }
  }
}
