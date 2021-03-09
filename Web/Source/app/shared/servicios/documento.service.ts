import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { ErrorHandler } from '../../core/http/error-handler.service';
import { Http, Headers, RequestOptions, Response } from '@angular/http';
import { Documento } from '../modelo/documento.model';
import { HttpUtils } from '../http-utils';
import { Documentacion } from '../modelo/documentacion.model';
import { Pagina, PaginaUtils } from '../paginacion/pagina-utils';
import { DocumentoConsulta } from '../modelo/consultas/documento-consulta.model';

@Injectable()
export class DocumentoService {
  private url: string = '/documentaciones';

  constructor(private http: Http, private errorHandler: ErrorHandler) {
  }

  public consultarHistorialArchivos(consulta: DocumentoConsulta): Observable<Pagina<Documento>> {
    return this.http.get(this.url, {search: HttpUtils.insertarPrefijo(consulta)})
      .map((res) => PaginaUtils.extraerPagina(Documento, res))
      .catch(this.errorHandler.handle);
  }

  public guardarDocumento(documentacion: Documentacion): Observable<number> {
    let headers = new Headers();
    let options = new RequestOptions({headers});
    let formData = HttpUtils.createFormData(documentacion);

    return this.http
      .post(this.url, formData, options)
      .map((response) => response.json().resultado)
      .catch(this.errorHandler.handle);
  }

  public consultarDocumento(documentoId: number, idItem: number): Observable<any> {
    return this.http
      .get(`${this.url}?idDocumento=${documentoId}&idItem=${idItem}`)
      .map((res: Response) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }
}
