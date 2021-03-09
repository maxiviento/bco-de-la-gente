import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs';
import { ErrorHandler } from '../../core/http/error-handler.service';
import { MapUtils } from '../../shared/map-utils';
import { HttpUtils } from '../../shared/http-utils';
import { Pagina, PaginaUtils } from '../../shared/paginacion/pagina-utils';
import { FiltrosUsuarios } from './modelo/filtros-usuarios.model';
import { Perfil } from './modelo/perfil.model';
import { ItemUsuario } from './modelo/item-usuario.model';
import { MotivoBaja } from './modelo/motivo-baja.model';

@Injectable()
export class UsuariosService {
  private static filtrosConsulta: FiltrosUsuarios;
  public url: string = '/usuarios';
  public urlCidi: string = '/usuarios/cidi';
  public urlPerfiles: string = '/perfiles/todos';
  public urlMotivosBaja: string = '/motivosbaja/usuarios';

  constructor(private http: Http,
              private errorHandler: ErrorHandler) {
  }

  public static guardarFiltros(filtros: FiltrosUsuarios): void {
    this.filtrosConsulta = filtros;
  }

  public static recuperarFiltros(): FiltrosUsuarios {
    return this.filtrosConsulta;
  }

  public consultarUsuarios(filtrosUsuarios: FiltrosUsuarios): Observable<Pagina<ItemUsuario[]>> {
    return this.http
      .get(this.url, {search: HttpUtils.insertarPrefijo(filtrosUsuarios)})
      .map((res) => {
        return PaginaUtils.extraerPagina(ItemUsuario, res);
      })
      .catch(this.errorHandler.handle);
  }

  public consultarUsuario(usuarioId: number): Observable<ItemUsuario> {
    let searchParams = new URLSearchParams();
    searchParams.set('usuarioId', usuarioId.toString());
    return this.http
      .get(`${this.url}/${usuarioId}`, {search: searchParams})
      .map(UsuariosService.extraerUsuario)
      .catch(this.errorHandler.handle);
  }

  private static extraerUsuario(res: Response): ItemUsuario {
    let resultado = res.json().resultado;
    if (resultado) {
      return MapUtils.deserialize(ItemUsuario, resultado);
    }
  }

  public consultarUsuarioCidi(cuil: number): Observable<ItemUsuario> {
    let searchParams = new URLSearchParams();
    searchParams.set('cuil', cuil.toString());
    return this.http
      .get(`${this.urlCidi}/${cuil}`, {search: searchParams})
      .map(UsuariosService.extraerUsuarioCidi)
      .catch(this.errorHandler.handle);
  }

  private static extraerUsuarioCidi(res: Response): ItemUsuario {
    let resultado = res.json().resultado;
    if (resultado) {
      return MapUtils.deserialize(ItemUsuario, resultado);
    }
  }

  public eliminarUsuario(usuarioId: number, motivoBajaId: string): Observable<void> {
    return this.http
      .put(`${this.url}/${usuarioId}/baja`, {motivoBajaId})
      .map((res) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public registrarUsuario(usuario: ItemUsuario): Observable<number> {
    return this.http
      .post(this.url, usuario)
      .map((res) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public modificarUsuario(usuarioId: number, perfilId: number): Observable<number> {
    return this.http
      .put(`${this.url}/${usuarioId}`, {perfilId})
      .map((res) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public obtenerMotivosBaja() {
    return this.http
      .get(this.urlMotivosBaja)
      .map(UsuariosService.extraerMotivoBaja)
      .catch(this.errorHandler.handle);
  }

  public static extraerMotivoBaja(res: Response): MotivoBaja {
    return (res.json().resultado || [])
      .map((motivoBaja) => MapUtils.deserialize(MotivoBaja, motivoBaja));
  }

  // agregar a perfiles.service
  public obtenerPerfiles() {
    return this.http
      .get(this.urlPerfiles)
      .map(UsuariosService.extraerPerfiles)
      .catch(this.errorHandler.handle);
  }

  public static extraerPerfiles(res: Response): Perfil {
    return (res.json().resultado || [])
      .map((perfil) => MapUtils.deserialize(Perfil, perfil));
  }
}
