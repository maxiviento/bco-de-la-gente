import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Http, Response } from '@angular/http';
import { ErrorHandler } from '../core/http/error-handler.service';

@Injectable()
export class RentasService {
  private url = '/rentas';

  constructor(private http: Http,
              private errorHandler: ErrorHandler) {
  }

  private static reporteSource: any;

  public static guardarReporte(reporte: any): void {
    this.reporteSource = reporte;
  }

  public static recuperarReporte(): any {
    return this.reporteSource;
  }

  public generarPdfGrupoFamiliar(filtros: any): Observable<string> {
    let params = new URLSearchParams();
    params.set('pais', filtros.pais);
    params.set('dni', filtros.dni);
    params.set('sexo', filtros.sexo);
    return this.http.get(this.url +
      `/reporte-grupo-familiar?pais=${filtros.pais}&dni=${filtros.dni}&sexo=${filtros.sexo}`)
      .map((res: Response) => {
        return res.json().resultado;
      }).catch(this.errorHandler.handle);
  }
}
