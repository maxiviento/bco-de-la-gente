import { Injectable } from '@angular/core';
import {
  ConnectionBackend,
  RequestOptions,
  Request,
  RequestOptionsArgs,
  Response,
  Http,
  Headers
} from '@angular/http';

import { Observable } from 'rxjs/Rx';
import { SpinnerService } from '../spinner/spinner.service';
import { SingleSpinnerService } from '../single-spinner/single-spinner.service';

const URLS_CONTADORES_INSCRIPCIONES = [
  '/consultar-totalizador',
  '/consultar-totalizador-suaf',
  '/consultar-totalizador-documentacion',
  '/consultar-totalizador-estado',
  '/consultar-totalizador-suaf',
  '/consultar-totalizador-cheque',
  '/consultar-totalizador-sucursal'
];
@Injectable()
export class HttpInterceptor extends Http {

  public pendingRequests: number = 0;
  public singlePendingRequests: number = 0;

  constructor(backend: ConnectionBackend,
              defaultOptions: RequestOptions,
              private spinnerService: SpinnerService,
              private singleSpinnerService: SingleSpinnerService) {
    super(backend, defaultOptions);
  }

  public request(url: string | Request, options?: RequestOptionsArgs): Observable<Response> {
    let url_req = url as Request;
    if (URLS_CONTADORES_INSCRIPCIONES.some((s) => {
      return url_req.url.indexOf(s) !== -1;
    })) {
      this.singlePendingRequests++;
      this.singleSpinnerService.show();
      return super.request(url, options)
        .flatMap(this.singleHandleResponse(url_req))
        .catch(this.singleHandleError(url_req));
    } else {
      this.pendingRequests++;
      this.spinnerService.show();
      return super.request(url, options)
        .flatMap(this.handleResponse())
        .catch(this.handleError());
    }
  }
  private singleHandleResponse(request: string | Request) {
    return (response: Response) => {
      this.singlePendingRequests--;
      this.updateSingleSpinner();
      return Observable.of(response);
    };
  }

  private singleHandleError(request: string | Request) {
    return (response: Response) => {

      this.singlePendingRequests--;
      this.updateSingleSpinner();

      if (response.status === 401 || response.status === 403) {
        delete localStorage['auth_token'];
      }
      return Observable.throw(response);
    };
  }
  private updateSingleSpinner(): void {
    if (this.singlePendingRequests === 0) {
      this.singleSpinnerService.hide();
    }
  }

  public get(url: string, options?: RequestOptionsArgs): Observable<Response> {
    url = this.updateUrl(url);
    return super.get(url, this.getRequestOptionArgs(options));
  }

  public post(url: string, body: string, options?: RequestOptionsArgs): Observable<Response> {
    url = this.updateUrl(url);
    return super.post(url, body, this.getRequestOptionArgs(options));
  }

  public put(url: string, body: string, options?: RequestOptionsArgs): Observable<Response> {
    url = this.updateUrl(url);
    return super.put(url, body, this.getRequestOptionArgs(options));
  }

  public delete(url: string, options?: RequestOptionsArgs): Observable<Response> {
    url = this.updateUrl(url);
    return super.delete(url, this.getRequestOptionArgs(options));
  }

  private updateUrl(req: string) {
    return API_URL + req;
  }

  private getRequestOptionArgs(options?: RequestOptionsArgs): RequestOptionsArgs {
    if (options == null) {
      options = new RequestOptions();
    }
    if (options.headers == null) {
      options.headers = new Headers();
      options.headers.set('Content-Type', 'application/json');
      options.headers.set('Accept', 'application/json');
    }
    if (this.token) {
      options.headers.set('Authorization', 'Bearer ' + this.token);
    }

    return options;
  }

  private get token(): string {
    return localStorage.getItem('auth_token');
  }

  private handleResponse() {
    return (response: Response) => {
      this.pendingRequests--;
      this.updateSpinner();
      return Observable.of(response);
    };
  }

  private handleError() {
    return (response: Response) => {
      this.pendingRequests--;
      this.updateSpinner();

      if (response.status === 401 || response.status === 403) {
        delete localStorage['auth_token'];
      }
      return Observable.throw(response);
    };
  }

  private updateSpinner(): void {
    if (this.pendingRequests === 0) {
      this.spinnerService.hide();
    }
  }
}
