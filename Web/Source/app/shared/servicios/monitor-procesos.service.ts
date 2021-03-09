import { Injectable } from "@angular/core";
import { ErrorHandler } from "../../core/http/error-handler.service";
import { Http } from "@angular/http";
import { Observable } from "rxjs";
import { EstadoProceso } from '../../prestamos-checklists/shared/modelos/estados-proceso.model';
import { TipoProceso } from '../../prestamos-checklists/shared/modelos/tipos-proceso.model';
import { ConsultaMonitorProcesos } from '../../prestamos-checklists/shared/modelos/consulta-procesos.model';
import { Pagina, PaginaUtils } from '../paginacion/pagina-utils';
import { BandejaMonitorResultado } from '../../prestamos-checklists/shared/modelos/bandeja-monitor-resultado.model';
import { Archivo } from '../modelo/archivo.model';
import { HttpUtils } from '../http-utils';
import { MapUtils } from '../map-utils';

@Injectable()

export class MonitorProcesoService {
  private url = '/monitorprocesos';
  private static filtrosConsulta: ConsultaMonitorProcesos;

  constructor(private http: Http,
              private errorHandler: ErrorHandler) {
  }

  public consultarBandejaMonitorProcesos(comando: ConsultaMonitorProcesos): Observable<Pagina<BandejaMonitorResultado>> {
    return this.http.post(`${this.url}/consultar-bandeja`, comando)
      .map((res) => {
        return PaginaUtils.extraerPagina(BandejaMonitorResultado, res);
      })
      .catch(this.errorHandler.handle);
  }

  public consultarTotalizador(consulta: ConsultaMonitorProcesos) {
    return this.http.post(`${this.url}/consultar-totalizador`, consulta)
      .map((res) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public cancelarProceso(idProceso: number): Observable<string>{
    return this.http.delete(`${this.url}/cancelar-proceso/${idProceso}`)
      .map((res) => {
        return res.json().resultado;
      })
      .catch(this.errorHandler.handle);
  }

  public static guardarFiltros(filtros: ConsultaMonitorProcesos): void {
    this.filtrosConsulta = filtros;
  }

  public static recuperarFiltros(): ConsultaMonitorProcesos {
    return this.filtrosConsulta;
  }

  public descargarReporte(consulta: BandejaMonitorResultado): Observable<Archivo>{
    return this.http.post(`${this.url}/descargar`, consulta)
    .map((res) => {
      return res.json().resultado;
    })
    .catch(this.errorHandler.handle);
  }
}
