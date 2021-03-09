import { Component, Input, OnInit } from '@angular/core';
import { BandejaSuafResultado } from '../../../shared/modelo/bandeja-suaf-resultado.model';
import * as FileSaver from 'file-saver';
import { ArchivoSuaf } from '../../../../etapas/shared/modelo/archivo-suaf.model';
import { PagosService } from '../../../shared/pagos.service';
import { NotificacionService } from '../../../../shared/notificacion.service';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { ELEMENTOS, Pagina } from '../../../../shared/paginacion/pagina-utils';
import { BandejaSuafConsulta } from '../../../shared/modelo/bandeja-suaf-consulta.model';
import { BandejaLoteResultado } from '../../../shared/modelo/bandeja-lote-resultado.model';
import { ProvidenciaComando } from '../../../shared/modelo/providencia-comando.model';
import { ModalFechaProvidenciaComponent } from '../../../modal-fecha-providencia/modal-fecha-providencia.component';
import { ArchivoService } from '../../../../shared/archivo.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component(
  {
    selector: 'bg-grilla-bandeja-suaf',
    templateUrl: 'grilla-bandeja-suaf.component.html',
    styleUrls: ['grilla-bandeja-suaf.component.scss']
  }
)
export class GrillaBandejaSuafComponent implements OnInit {
  public archivoSuaf: ArchivoSuaf = new ArchivoSuaf();
  public bandejaResultados: BandejaSuafResultado [] = [];
  private _consulta: BandejaSuafConsulta;
  public excel = new BehaviorSubject<SafeResourceUrl>(null);
  public pdfProv = new BehaviorSubject<SafeResourceUrl>(null);
  public pagina: Observable<Pagina<any>> = new Observable<Pagina<any>>();
  public paginaModificada = new Subject<number>();
  public reporteSource: any;

  @Input()
  public set consulta(consulta: BandejaSuafConsulta) {
    this._consulta = consulta;
    if (this._consulta) {
      this.paginaModificada.next(this._consulta.numeroPagina);
    }
  }

  @Input() public totalizador: number;
  constructor(private pagosService: PagosService,
              private archivoService: ArchivoService,
              private notificacionService: NotificacionService,
              private sanitizer: DomSanitizer,
              public modalService: NgbModal) {
    this.configurarPaginacion();
  }

  public ngOnInit() {
  }

  private configurarPaginacion() {
    this.pagina = this.paginaModificada
      .map((numeroPagina) => {
        return {numeroPagina};
      })
      .flatMap((params: { numeroPagina: number }) => {
        let filtros = this._consulta;
        filtros.numeroPagina = params.numeroPagina;
        return this.pagosService.consultarBandejaSuaf(filtros);
      })
      .share();
    (<Observable<BandejaLoteResultado[]>> this.pagina.pluck(ELEMENTOS))
      .subscribe((resultado) => {
        this.bandejaResultados = resultado;
      });
  }

/*  public consultarTotalizador(filtros: BandejaSuafConsulta) {
    this.totalizador = 0;
    this.pagosService
      .consultarTotalizadorSuaf(filtros)
      .subscribe((num) => this.totalizador = num);
  }*/

  public imprimirExcel(idLote: number): void {
    this.pagosService.generarReporteExcelSuaf(idLote).subscribe((res) => {
      this.excel.next(this.sanitizer.bypassSecurityTrustResourceUrl(res));
      this.reporteSource = this.excel.getValue();
      let arrayBytes = this.base64ToBytes(res.blob);
      let blob = new Blob([arrayBytes], {type: ''});
      FileSaver.saveAs(blob, res.fileName);
    });
  }

  public imprimirExcelActivacionMasiva(idLote: number): void {
    this.pagosService.generarExcelActivacionMasiva(idLote).subscribe((res) => {
      this.excel.next(this.sanitizer.bypassSecurityTrustResourceUrl(res));
      this.reporteSource = this.excel.getValue();
      let arrayBytes = this.base64ToBytes(res.blob);
      let blob = new Blob([arrayBytes], {type: ''});
      FileSaver.saveAs(blob, res.fileName);
    });
  }

  public imprimirProvidenciaMasiva(idLote: number) {
    this.obtenerProvidenciaMasiva(idLote).then();
  }

  public async obtenerProvidenciaMasiva(idLote: number) {
    const modalRef = await this.modalService.open(ModalFechaProvidenciaComponent, {
      backdrop: 'static',
      size: 'lg',
      keyboard: false
    });
    modalRef.componentInstance.idFormulario = -1;
    modalRef.result.then((res) => {
      if (res) {
        let providenciaComando = new ProvidenciaComando();
        if (res.fechaAprovacionMasiva) {
          providenciaComando.fechaAprovacionMasiva = true;
          providenciaComando.fechaManual = false;
        } else {
          providenciaComando.fechaManual = true;
          providenciaComando.fechaAprovacionMasiva = false;
          providenciaComando.fecha = res.fecha;
        }
        providenciaComando.idLote = idLote;
        this.pagosService.generarProvidenciaMasiva(providenciaComando)
          .subscribe((resultado) => {
            if (resultado) {
              if (resultado.errores && resultado.errores.length > 0) {
                this.notificacionService.informar(resultado.errores, true);
                return;
              }
              this.pdfProv.next(this.sanitizer.bypassSecurityTrustResourceUrl(this.archivoService.getUrlPrevisualizacionArchivo(resultado.archivos[0])));
              this.archivoService.descargarArchivo(resultado.archivos[0]);
            }
          }, (errores) => {
            this.notificacionService.informar(errores, true);
          });
      }
    });
  }

  private base64ToBytes(base64) {
    let raw = window.atob(base64);
    let n = raw.length;
    let bytes = new Uint8Array(new ArrayBuffer(n));

    for (let i = 0; i < n; i++) {
      bytes[i] = raw.charCodeAt(i);
    }
    return bytes;
  }

  public archivoSeleccionado(archivo: File, idLote: number): void {
    this.archivoSuaf.Archivo = archivo;
    this.archivoSuaf.LoteId = idLote;
    this.pagosService.registrarArchivoSuaf(this.archivoSuaf).subscribe((resultado) => {
        this.notificacionService.informar(['Cantidad de registros devengados: ' + resultado.cantidadDevengados,
            'Cantidad de registros no procesados: ' + resultado.cantidadNoProcesados],
          false,
          'Archivo procesado con éxito');
      },
      (errores) => {
        this.notificacionService.informar(errores, true);
      });
  }

  public consultarSiguientePagina(pagina?: number): void {
    this.paginaModificada.next(pagina);
  }
}
