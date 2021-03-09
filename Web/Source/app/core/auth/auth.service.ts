import {Injectable} from '@angular/core';
import {Headers, Http, RequestOptions, Response} from '@angular/http';
import {BehaviorSubject, Observable} from 'rxjs';
import {Token} from './modelos/token.model';
import {MapUtils} from '../../shared/map-utils';

@Injectable()
export class AuthService {

  private url = '/autenticacion/token';
  private usuarioInicioSesion = new BehaviorSubject<boolean>(this.hayUnToken());
  private grantType: string = 'cidi';

  constructor(private http: Http) {

  }

  public iniciarSesion(): Observable<boolean> {
    let headers = new Headers({'Content-Type': 'application/x-www-form-urlencoded'});
    let options = new RequestOptions({headers: headers});
    return this.http.post(this.url, this.urlEncoded(), options)
      .map((res: Response) => {
        let resultado = res.json();
        let token = MapUtils.deserialize(Token, resultado);
        localStorage.setItem('auth_token', token.accessToken);
        this.usuarioInicioSesion.next(true);
        return true;
      })
      .catch((res) => {
        window.location.href = res.headers.get('x-auth-login-path') || '/';
        return Observable.throw(res);
      });
  }

  private urlEncoded(): string {
    return `grant_type=${this.grantType}&client_id=${KEY_APP}`;
  }

  private hayUnToken(): boolean {
    return !!this.token();
  }

  public token(): String {
    return localStorage.getItem('auth_token');
  }

  public estaLogueado(): Observable<boolean> {
    return this.usuarioInicioSesion.asObservable().share();
  }
}
