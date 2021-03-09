import { Component, Input } from '@angular/core';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { ELEMENTOS, Pagina } from '../../../shared/paginacion/pagina-utils';
import { BandejaLotesConsulta } from '../../shared/modelo/bandeja-lotes-consulta.model';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { PagosService } from '../../shared/pagos.service';
import { NotificacionService } from '../../../shared/notificacion.service';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { BandejaLoteResultado } from '../../shared/modelo/bandeja-lote-resultado.model';
import * as FileSaver from 'file-saver';
import { ModalNotaPagosComponent } from '../../modal-nota-pagos/modal-nota-pagos.component';
import { ModalModalidadPagoComponent } from '../../modal-modalidad-pago/modal-modalidad-pago.component';
import { ArchivoService } from '../../../shared/archivo.service';
import { ModalTipoPagosComponent } from '../../modal-tipo-pagos/modal-tipo-pagos.component';
import {Router} from "@angular/router";

@Component({
  selector: 'bg-grilla-bandeja-lotes',
  templateUrl: 'grilla-bandeja-lotes.component.html',
  styleUrls: ['grilla-bandeja-lotes.component.scss']
})
export class GrillaBandejaLotesComponent {
  private _consulta: BandejaLotesConsulta;
  public bandejaResultados: BandejaLoteResultado[] = [];
  public excelBanco = new BehaviorSubject<SafeResourceUrl>(null);
  public archivoTxt = new BehaviorSubject<SafeResourceUrl>(null);
  public formLotes: FormGroup;
  public pagina: Observable<Pagina<any>> = new Observable<Pagina<any>>();
  public paginaModificada = new Subject<number>();
  public reporteSource: any;
  public estadoPrestamoValido: boolean;

  @Input()
  public set consulta(consulta: BandejaLotesConsulta) {
    this._consulta = consulta;
    if (this._consulta) {
      this.paginaModificada.next(this._consulta.numeroPagina);
    }
  }

  @Input() public totalizador: number;

  constructor(private fb: FormBuilder,
              private pagosService: PagosService,
              private notificacionService: NotificacionService,
              private archivoService: ArchivoService,
              private sanitizer: DomSanitizer,
              private router: Router,
              private modalService: NgbModal) {
    this.configurarPaginacion();
    this.crearFormLotes();
  }

  private configurarPaginacion() {
    this.pagina = this.paginaModificada
      .map((numeroPagina) => {
        return {numeroPagina};
      })
      .flatMap((params: { numeroPagina: number }) => {
        let filtros = this._consulta;
        filtros.numeroPagina = params.numeroPagina;
        this.pagosService.consultarTotalizador(filtros).subscribe((total) => this.totalizador = total
        );
        return this.pagosService.consultarBandejaLotes(filtros);
      })
      .share();
    (<Observable<BandejaLoteResultado[]>> this.pagina.pluck(ELEMENTOS))
      .subscribe((resultado) => {
        this.bandejaResultados = resultado;
        this.crearFormLotes();
      }, (errores) => {
        this.notificacionService.informar(errores, true);
      });

  }

  private crearFormLotes() {
    this.formLotes = this.fb.group({
      lotes: this.fb.array((this.bandejaResultados || []).map((lote) =>
        this.fb.group({
          idLote: [lote.idLote],
          fechaLote: [lote.fechaLote],
          nroLote: [lote.nroLote],
          nombreLote: [lote.nombreLote],
          cantPrestamos: [lote.cantPrestamos],
          cantBeneficiarios: [lote.cantBeneficiarios],
          montoTotal: [lote.montoTotal],
          comision: [lote.comision],
          idTipoLote: [lote.idTipoLote],
          iva: [lote.iva],
          permiteLiberar: [lote.permiteLiberar]
        })
      )),
    });
  }

  public get lotesFormArray(): FormArray {
    return this.formLotes.get('lotes') as FormArray;
  }

  public imprimirExcel(idLote: number) {
    this.pagosService.validarProvidenciaLote(idLote).subscribe((res) => {
      if (!res) {
        this.pagosService.generarReporteExcelBanco(idLote).subscribe((res) => {
          this.excelBanco.next(this.sanitizer.bypassSecurityTrustResourceUrl(res));
          this.reporteSource = this.excelBanco.getValue();
          let arrayBytes = this.base64ToBytes(res.blob);
          let blob = new Blob([arrayBytes], {type: ''});
          FileSaver.saveAs(blob, res.fileName);
          this.validarEstadoPrestamo(idLote);
        });
      } else {
        this.generarTxtListadoForms(res);
        this.notificacionService.informar(['Algun formulario del lote no tiene generada providencia'], true);
      }
    }, (errores) => {
      this.notificacionService.informar(errores, true);
    });
  }

  public imprimirExcelCheques(idLote: number) {
    this.pagosService.validarProvidenciaLote(idLote).subscribe((res) => {
      if (!res) {
        this.pagosService.generarReporteExcelBanco(idLote).subscribe((res) => {
          this.excelBanco.next(this.sanitizer.bypassSecurityTrustResourceUrl(res));
          this.reporteSource = this.excelBanco.getValue();
          let arrayBytes = this.base64ToBytes(res.blob);
          let blob = new Blob([arrayBytes], {type: ''});
          FileSaver.saveAs(blob, res.fileName);
          this.pagosService.registrarChequeFormularios(idLote).subscribe((res) => {
          }, (errores) => {
            this.notificacionService.informar(errores, true);
          });
        }, (errores) => {
          this.notificacionService.informar(errores, true);
        });
      } else {
        this.generarTxtListadoForms(res);
        this.notificacionService.informar(['Algun formulario del lote no tiene generada providencia'], true);
      }
    }, (errores) => {
      this.notificacionService.informar(errores, true);
    });
  }

  private generarTxtListadoForms(archivo: any) {
    this.archivoTxt.next(this.sanitizer.bypassSecurityTrustResourceUrl(archivo));
    this.reporteSource = this.archivoTxt.getValue();
    let arrayBytes = this.base64ToBytes(archivo.blob);
    let blob = new Blob([arrayBytes], {type: ''});
    FileSaver.saveAs(blob, archivo.fileName);
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

  public imprimirNota(idLote: number) {
    const modalRef = this.modalService.open(ModalNotaPagosComponent, {backdrop: 'static'});
    modalRef.componentInstance.idLote = idLote;
  }

  public actualizarModalidadPago(idLote: number) {
    const modalRef = this.modalService.open(ModalModalidadPagoComponent, {backdrop: 'static', windowClass: 'modal-l'});
    modalRef.componentInstance.idLote = idLote;
  }

  public consultarSiguientePagina(pagina?: number): void {
    this.paginaModificada.next(pagina);
  }

  public seleccionarTipoPago(idLote: number) {
    if (this.estadoPrestamoValido) {
      const modalRef = this.modalService.open(ModalTipoPagosComponent, {backdrop: 'static'});
      modalRef.componentInstance.idLote = idLote;
    } else {
      this.notificacionService.informar(['No hay ningún préstamo en estado correcto para generar el txt.'], true);
    }

  }

  public validarEstadoPrestamo(idLote: number): any {
    this.pagosService.validarEstadoFormulario(idLote).subscribe((value) => {
      this.estadoPrestamoValido = value;
      this.seleccionarTipoPago(idLote);
    });
  }
}
