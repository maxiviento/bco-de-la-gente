import { Response } from '@angular/http';
import { Observable } from 'rxjs';
import 'rxjs/add/observable/throw';
import { Injectable } from '@angular/core';
import { LoggerService } from '../logger.service';

@Injectable()
export class ErrorHandler {

  constructor(private logger: LoggerService) {
  }

  /* TODO: errorControlado == true se usa para los errores que tienen titulo y cuerpo especificado por la excepcion. Los false tienen mensaje y titulo generico.*/

  public handle(error: Response | any) {
    console.error(error);
    if (error instanceof Response) {
      let body = error.json();
      if ((error.status === 400 && body.errorControlado) || (error.status === 500)) {
        return Observable.throw(body.errores.map((error) => error.titulo));
      }
    }
    if (typeof error === 'string') {
      this.logger.error(error);
    }
    return Observable.throw(['Hubo un error de parte nuestra.']);
  }
}
