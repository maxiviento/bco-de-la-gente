import { Injectable } from '@angular/core';
import * as CryptoJS from 'crypto-js';
import { Http, Response } from '@angular/http';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import 'rxjs/add/observable/of';
import { Observable, BehaviorSubject } from 'rxjs';
import { MapUtils } from '../../shared/map-utils';
import { Usuario } from './modelos/usuario.model';
import { AuthService } from './auth.service';
import { Permiso } from './modelos/permiso.model';
import { UsuarioLogueado } from './modelos/usuario-logueado.model';
import { ErrorHandler } from '../http/error-handler.service';

@Injectable()
export class UsuarioService {

  private url = '/usuarios';
  private usuarioActual = new BehaviorSubject<Usuario>(null);
  private permisos = new BehaviorSubject<Permiso[]>([]);
  private static _authService: AuthService;
  constructor(private http: Http,
              private authService: AuthService,
              private errorHandler: ErrorHandler) {
    UsuarioService._authService = authService;
    authService
      .estaLogueado()
      .subscribe((valor) => {
        if (valor) {
          this.consultarUsuario()
            .subscribe((usuario) => {
              this.usuarioActual.next(usuario);
            });
          this.consultarPermisos()
            .subscribe((permisos) => {
              this.permisos.next(permisos);
            });
        } else {
          this.usuarioActual.next(null);
          this.permisos.next([]);
          localStorage.removeItem('permission');
          authService
            .iniciarSesion()
            .subscribe();
        }
      });
  }

  private consultarPermisos(): Observable<Permiso[]> {
    let funcionalidades = localStorage.getItem('permission');

    if (!funcionalidades) {
      return this.http
        .get(this.url + '/yo/permisos')
        .map((res: Response) => {
          this.guardarPermisos(res.json().resultado);
          return (res.json().resultado || [])
            .map((permiso) => MapUtils.deserialize(Permiso, permiso));
        })
        .catch((res: Response) => {
          let href = res.headers.get('x-auth-login-path') || '/';
          window.location.href = href;
          return Observable.throw(res);
        });
    } else {
      let bytes = CryptoJS.AES.decrypt(funcionalidades, 'deRhGfhtDsFsdFsfsa');
      let localStoragePermisos = JSON.parse(bytes.toString(CryptoJS.enc.Utf8));
      return Observable.of(localStoragePermisos);
    }
  }

  private guardarPermisos(resultado: Permiso[]): void {
    let allowed = CryptoJS.AES.encrypt(JSON.stringify(resultado), 'deRhGfhtDsFsdFsfsa').toString();

    localStorage.setItem('permission', allowed);
  }

  private consultarUsuario(): Observable<Usuario> {
    return this.http
      .get(this.url + '/yo')
      .map(UsuarioService.extraerUsuario)
      .catch((res: Response) => {
        let href = res.headers.get('x-auth-login-path') || '/';
        window.location.href = href;
        return Observable.throw(res);
      });
  }

  public consultarUsuarioLogueado(): Observable<UsuarioLogueado> {
    return this.http
      .get(this.url + '/logueado')
      .map((res) => MapUtils.extractModel(UsuarioLogueado, res))
      .catch(this.errorHandler.handle);
  }

  private static extraerUsuario(res: Response): Usuario {
    let resultado = res.json().resultado;
    let usuario = resultado ? MapUtils.deserialize(Usuario, resultado) : undefined;
    if (usuario && usuario.reiniciarToken) {
      delete localStorage['auth_token'];
      UsuarioService._authService
        .iniciarSesion()
        .subscribe();
    }
    if (resultado) {
      return usuario;
    }
  }

  public obtenerUsuarioActual(): Observable<Usuario> {
    return this.usuarioActual.asObservable().share();
  }

  public tienePermiso(codigo: string): Observable<boolean> {
    return this.permisos
      .asObservable()
      .find((permisos: Permiso[]) => {
        return permisos.some((permiso) => codigo === permiso.codigo);
      })
      .flatMap((permisos) => {
        return Observable.of(permisos.length > 0);
      })
      .share();
  }

  public tienePermisoFuncionalidad(codigo: string): Observable<boolean> {
    return this.permisos
      .asObservable()
      .flatMap((permisos) => {
        return Observable.of(permisos.some((permiso) => permiso.codigo === codigo));
      })
      .share();
  }

  public tienePermisoLista(codigos: string[]): Observable<boolean> {
    return this.permisos
      .asObservable()
      .first((permisos: Permiso[]) => {
        return permisos.some((permiso) => {
          return codigos.some((codigo) => {
            return permiso.codigo.includes(codigo);
          });
        });
      })
      .flatMap((permisos) => {
        return Observable.of(permisos.length > 0);
      })
      .share();
  }

  public cerrarSesionCidi(): Observable<string> {
    return this.http
      .get(this.url + '/ObtenerUrlCerrarSesion')
      .map((res: Response) => {
        return res.json().resultado;

      })
      .catch((res: Response) => {
        return Observable.throw(res);
      });
  }
}
