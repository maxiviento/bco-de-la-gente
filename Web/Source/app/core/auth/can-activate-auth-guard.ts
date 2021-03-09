import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot, UrlSegment } from '@angular/router';
import { AuthService } from './auth.service';
import { Observable } from 'rxjs';
import 'rxjs/operator/map';
import 'rxjs/operator/catch';
import 'rxjs/add/observable/of';
import { UsuarioService } from './usuario.service';

@Injectable()
export class CanActivateAuthGuard implements CanActivate {

  constructor(private router: Router, private authService: AuthService, private usuarioService: UsuarioService) {
  }

  public canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    return this.authService
      .estaLogueado()
      .debounceTime(200)
      .map((logueado) => {
        if (!logueado) {
          this.router.navigate(['/'], {queryParams: {returnUrl: state.url}});
        }
        return logueado;
      }).concatMap((logueado) => this.usuarioService.tienePermisoFuncionalidad(this.obtenerCodigoFuncionalidad(route.url)))
      .map((tienePermiso) => tienePermiso)
      .catch(() => {
        this.router.navigate(['/'], {queryParams: {returnUrl: state.url}});
        return Observable.of(false);
      });
  }

  private obtenerCodigoFuncionalidad(segmentosUrl: UrlSegment[]): string {
    let concat = '';
    for (let i = 0; i < segmentosUrl.length; i++) {
      if (isNaN(parseInt(segmentosUrl[i].path, 10))) {
        concat += '/' + segmentosUrl[i].path;
      } else {
        concat += '/:id';
      }
    }
    return concat;
  }
}
