import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NotificacionService } from '../../../../shared/notificacion.service';
import { BehaviorSubject } from 'rxjs';
import { SafeResourceUrl, DomSanitizer } from '@angular/platform-browser';
import { PagosService } from '../../../../pagos/shared/pagos.service';
import { ArchivoService } from '../../../../shared/archivo.service';
import { ReportesDocumentacionPagos } from '../../modelos/reporte-documentacion-pagos-enum';
import { CustomValidators } from '../../../../shared/forms/custom-validators';
import { AuditoriaService } from '../../../../formularios/shared/auditoria.service';
import { Auditoria } from '../../../../shared/modelo/auditoria.modelo';
import { AuditoriaAccionEnum } from '../../modelos/auditoria-enum.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ModalFechaDocumentosComponent } from '../modal-fecha-documentos/modal-fecha-documento.component';

@Component({
  selector: 'bg-documentacion-pagos',
  templateUrl: './documentacion-pagos.component.html',
  styleUrls: ['./documentacion-pagos.component.scss'],
  providers: [AuditoriaService]
})

export class DocumentacionPagosComponent implements OnInit {
  @Input() public idsFormularios: number [] = [];
  @Input() public idLote: number;
  @Input() public idFormularioLinea: number;
  @Input() public esConApoderado: boolean;
  @Output() public clickVolver: EventEmitter<boolean> = new EventEmitter<boolean>();

  public form: FormGroup;
  public pdfPagos = new BehaviorSubject<SafeResourceUrl>(null);
  public procesando: boolean = false;
  public reportesPagos: any[] = [];
  private reportesAImprimir: number[] = [];
  public fecha: Date;
  public fechaAprobacion: boolean = false;

  constructor(private fb: FormBuilder,
              private router: Router,
              private sanitizer: DomSanitizer,
              private pagosServicio: PagosService,
              private archivoService: ArchivoService,
              private notificacionService: NotificacionService,
              private auditoriaService: AuditoriaService,
              public modalService: NgbModal) {
    this.reportesPagos = [
      {id: ReportesDocumentacionPagos.Caratula, descripcion: 'Carátula', seleccionado: false},
      {id: ReportesDocumentacionPagos.Recibo, descripcion: 'Recibo', seleccionado: false},
      {id: ReportesDocumentacionPagos.Pagare, descripcion: 'Pagaré', seleccionado: false},
      {id: ReportesDocumentacionPagos.Providencia, descripcion: 'Providencia', seleccionado: false},
      {id: ReportesDocumentacionPagos.ContratoMutuo, descripcion: 'Contrato mutuo', seleccionado: false}];
  }

  public ngOnInit(): void {
    this.crearForm();
  }

  private crearForm(): void {
    this.form = this.fb.group({
      idOpcion: ['', Validators.required],
      nombreDocumento: new FormControl('', CustomValidators.fileName)
    });

    this.form.get('idOpcion')
      .valueChanges
      .distinctUntilChanged()
      .subscribe((value) => {
        if (value) {
          if (value === 2) {
            this.reportesAImprimir = [];
            this.form.get('nombreDocumento').setValue('', {emitEvent: false});
          }
        }
      });
  }

  public formulariosNoImpresos() {
    this.procesando = true;
    this.pagosServicio.reporteFormulariosNoImpresos({
      idsFormularios: this.idsFormularios,
      idFormularioLinea: this.idFormularioLinea,
      idLote: this.idLote,
      idOpcion: this.form.value.idOpcion,
      idsReportesPagos: this.reportesAImprimir,
      fechaAprobacion: this.fechaAprobacion,
      fecha: this.fecha
    })
      .subscribe((resultado) => {
        if (resultado) {
          this.procesando = false;
          if (resultado.errores && resultado.errores.length > 0) {
            this.notificacionService.informar(resultado.errores, true);
            return;
          }
          this.archivoService.descargarArchivos(resultado.archivos);
        }
      }, (errores) => {
        this.procesando = false;
        this.notificacionService.informar(errores, true);
      });
  }

  public async confirmarImpresion() {
    if (this.form.valid && this.validarImpresion()) {
      const modalRef = await this.modalService.open(ModalFechaDocumentosComponent, {
        backdrop: 'static',
        size: 'lg',
        keyboard: false
      });
      modalRef.componentInstance.idFormulario = -1;
      modalRef.result.then((res) => {
        if (res) {
          this.fechaAprobacion = res.fechaAprobacion;
          this.fecha = res.fecha;
          this.procesando = true;
          this.pagosServicio.reporteDocumentacionPagos({
            idsFormularios: this.idsFormularios,
            idFormularioLinea: this.idFormularioLinea,
            idLote: this.idLote,
            idOpcion: this.form.value.idOpcion,
            idsReportesPagos: this.reportesAImprimir,
            fechaAprobacion: this.fechaAprobacion,
            fecha: this.fecha
          })
            .subscribe((resultado) => {
              if (resultado) {
                this.procesando = false;
                if (resultado.errores && resultado.errores.length > 0) {
                  this.notificacionService.informar(resultado.errores, true);
                  return;
                }
                this.pdfPagos.next(this.sanitizer.bypassSecurityTrustResourceUrl(this.archivoService.getUrlPrevisualizacionArchivo(resultado.archivos[0])));
                this.archivoService.descargarArchivos(resultado.archivos, this.conformarNombreDelReporte());
                this.generarSeguimiento();
              } else {
                {
                  this.notificacionService.informar(['Los documentos seleccionados fueron agregados a la cola de impresión, los mismos los podrá visualizar el en monitor de procesos.'], false);
                }
              }
            }, (errores) => {
              this.procesando = false;
              this.notificacionService.informar(errores, true);
            });
          this.formulariosNoImpresos();
        }
      });
    }
  }

  public checkReporte(id: number, event) {
    if (!this.reportesAImprimir.some((Id) => Id === id) && event.target.checked) {
      this.reportesAImprimir.push(id);
    }
    if (this.reportesAImprimir.some((Id) => Id === id) && !event.target.checked) {
      let i = this.reportesAImprimir.indexOf(id);
      this.reportesAImprimir.splice(i, 1);
    }
  }

  private conformarNombreDelReporte(): string {
    let formModel = this.form.value;
    let nombreReporte: string;

    if (formModel.idOpcion === 2) {
      nombreReporte = 'Cuponera';
    } else {
      if (this.reportesAImprimir.length === 1) {
        nombreReporte = this.reportesPagos.find((elem) => {
          return elem.id === this.reportesAImprimir[0];
        }).descripcion;
      } else if (this.reportesAImprimir.length > 1) {
        nombreReporte = formModel.nombreDocumento;
      }
    }
    return nombreReporte;
  }

  private validarImpresion(): boolean {
    if (this.form.get('idOpcion').value === 1) {
      if (!this.reportesAImprimir.length) {
        this.notificacionService.informar(['Debe seleccionar al menos un reporte a imprimir']);
        return false;
      }
    }
    return true;
  }

  public cerrar(): void {
    this.router.navigate(['/']);
  }

  public volver(): void {
    this.clickVolver.emit();
  }

  public generarSeguimiento(): void {
    this.idsFormularios.forEach((idsForm) => {
      let idOpcion = this.form.value.idOpcion;
      if (idOpcion === 2) {
        let auditoria = new Auditoria();
        auditoria.idFormularioLinea = idsForm;
        auditoria.idAccion = AuditoriaAccionEnum.IMPRESION_CUPONERA;
        this.auditoriaService.registrarSeguimiento(auditoria).subscribe();
      } else {
        if (idOpcion === 1) {
          let auditoria = new Auditoria();
          auditoria.idFormularioLinea = idsForm;
          this.reportesAImprimir.forEach((reportesImprimir) => {
            auditoria.idAccion = reportesImprimir;
            this.auditoriaService.registrarSeguimiento(auditoria).subscribe();
          });
        }
      }
    });
  }
}
