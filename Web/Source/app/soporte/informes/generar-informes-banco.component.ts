import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbUtils } from '../../shared/ngb/ngb-utils';
import { CustomValidators } from '../../shared/forms/custom-validators';
import { ConsultaInformePagos } from '../modelo/consulta-informe-pagos';
import { PagosService } from '../../pagos/shared/pagos.service';
import * as FileSaver from 'file-saver';
import { BehaviorSubject } from 'rxjs';
import { DomSanitizer, SafeResourceUrl, Title } from '@angular/platform-browser';
import { Auditoria } from '../../shared/modelo/auditoria.modelo';
import { AuditoriaAccionEnum } from '../../seleccion-formularios/shared/modelos/auditoria-enum.model';
import { InformesBancoEnum } from '../modelo/informes-banco-enum';
import { AuditoriaService } from '../../formularios/shared/auditoria.service';
import { DateUtils } from '../../shared/date-utils';
import { NgbDatepickerConfig } from '@ng-bootstrap/ng-bootstrap';
import TituloBanco from '../../shared/titulo-banco';
import { ArchivoService } from "../../shared/archivo.service";
import { NotificacionService } from '../../shared/notificacion.service';

@Component({
  selector: 'bg-generar-informes-banco',
  templateUrl: './generar-informes-banco.component.html',
  styleUrls: ['./generar-informes-banco.component.scss'],
  providers: [AuditoriaService]
})

export class GenerarInformesBancoComponent implements OnInit {
  public form: FormGroup;
  public informesBanco: any[] = [];
  private informesAImprimir: number[] = [];
  public excel = new BehaviorSubject<SafeResourceUrl>(null);
  public reporteSource: any;
  public tipoReporte: number = 0;
  public exportacionesBanco: any[] = [];

  constructor(private pagoService: PagosService,
              private auditoriaService: AuditoriaService,
              private archivoService: ArchivoService,
              private sanitizer: DomSanitizer,
              private configFechas: NgbDatepickerConfig,
              private titleService: Title,
              private notificacionService: NotificacionService) {

    this.titleService.setTitle('Generar informes banco ' + TituloBanco.TITULO);
    this.informesBanco = [
      {id: InformesBancoEnum.CuadroCreditos, descripcion: 'Cuadro créditos', seleccionado: false},
      {id: InformesBancoEnum.CuadroPagados, descripcion: 'Cuadro de los pagados', seleccionado: false},
      {id: InformesBancoEnum.HistoricosPagados, descripcion: 'Históricos pagados', seleccionado: false},
      {id: InformesBancoEnum.CuadroProyectados, descripcion: 'Cuadro de los proyectados', seleccionado: false},
      {
        id: InformesBancoEnum.ProyectadosDptosLocalidades,
        descripcion: 'Proyectados dptos-localidades',
        seleccionado: false
      }];

    this.exportacionesBanco = [
      {id: InformesBancoEnum.ExportacionPrestamos, descripcion: 'Exportación de prestamos', seleccionado: false},
      {id: InformesBancoEnum.ExportacionRecupero, descripcion: 'Exportación de recupero', seleccionado: false}
    ];
  }

  public ngOnInit() {
    this.crearForm();
    this.limitarFechas();
  }

  private limitarFechas() {
    DateUtils.setMaxDateDP(new Date(), this.configFechas);
  }

  public generarInformesBanco(): void {
    let form = this.form.value;
    let consulta = new ConsultaInformePagos(this.informesAImprimir, NgbUtils.obtenerDate(form.fechaDesde), NgbUtils.obtenerDate(form.fechaHasta));
    this.pagoService.generarInformesBanco(consulta).subscribe((res) => {
      if  (res){
        this.excel.next(this.sanitizer.bypassSecurityTrustResourceUrl(this.archivoService.getUrlPrevisualizacion(res.blob, 'application/vnd.ms-excel')));
        this.reporteSource = this.excel.getValue();
        let arrayBytes = this.base64ToBytes(res.blob);
        let blob = new Blob([arrayBytes], {type: ''});
        FileSaver.saveAs(blob, res.fileName);
      } else {
        this.notificacionService.informar(['Los informes seleccionados fueron agregado a la cola de impresión, los mismos los podrá visualizar en monitor de procesos.'], false);
      }

      this.generarSeguimiento();
    });
  }

  public generarSeguimiento(): void {
    this.informesAImprimir.forEach((idInformes) => {
      let auditoria = new Auditoria();
      if (idInformes === InformesBancoEnum.CuadroCreditos) {
        auditoria.idAccion = AuditoriaAccionEnum.CUADRO_CREDITOS;
        this.auditoriaService.registrarSeguimiento(auditoria).subscribe();
      } else {
        if (idInformes === InformesBancoEnum.CuadroPagados) {
          auditoria.idAccion = AuditoriaAccionEnum.CUADRO_PAGADOS;
          this.auditoriaService.registrarSeguimiento(auditoria).subscribe();
        } else {
          if (idInformes === InformesBancoEnum.HistoricosPagados) {
            auditoria.idAccion = AuditoriaAccionEnum.HISTORICOS_PAGADOS;
            this.auditoriaService.registrarSeguimiento(auditoria).subscribe();
          } else {
            if (idInformes === InformesBancoEnum.CuadroProyectados) {
              auditoria.idAccion = AuditoriaAccionEnum.CUADRO_PROYECTADOS;
              this.auditoriaService.registrarSeguimiento(auditoria).subscribe();
            } else {
              if (idInformes === InformesBancoEnum.ProyectadosDptosLocalidades) {
                auditoria.idAccion = AuditoriaAccionEnum.PROYECTADOS_DPTOS_LOCALIDADES;
                this.auditoriaService.registrarSeguimiento(auditoria).subscribe();
              }
            }
          }
        }
      }
    });
  }

  private crearForm() {
    let fechaDesdeFc = new FormControl(NgbUtils.obtenerNgbDateStruct(new Date(Date.now())),
      Validators.required);
    let fechaHastaFc = new FormControl(NgbUtils.obtenerNgbDateStruct(new Date(Date.now())),
      Validators.required);
    let radioReporte = new FormControl(0);
    radioReporte.valueChanges.debounceTime(100)
      .subscribe((value) => {
          this.tipoReporte = value;
          this.informesAImprimir = [];
        }
      );
    fechaDesdeFc.valueChanges.debounceTime(500).subscribe((value) => {
      if (NgbUtils.obtenerDate(value)) {
        fechaHastaFc.clearValidators();
        let fechaDesdeMilisec = Date.parse(NgbUtils.obtenerDate(value).toISOString());
        let fechaActualMilisec = Date.parse(new Date().toISOString());
        let minDate;
        fechaDesdeMilisec <= fechaActualMilisec ? minDate = new Date(fechaDesdeMilisec) : minDate = new Date(fechaActualMilisec);
        fechaHastaFc.setValidators(Validators.compose([CustomValidators.minDate(minDate), Validators.required,
          CustomValidators.maxDate(new Date())]));
        fechaHastaFc.updateValueAndValidity();
      }
    });
    fechaHastaFc.valueChanges.debounceTime(500).subscribe((value) => {
      if (NgbUtils.obtenerDate(value)) {
        fechaDesdeFc.clearValidators();
        let fechaHastaMilisec = Date.parse(NgbUtils.obtenerDate(value).toISOString());
        let fechaActualMilisec = Date.parse(new Date().toISOString());
        let maxDate;
        fechaHastaMilisec <= fechaActualMilisec ? maxDate = new Date(fechaHastaMilisec) : maxDate = new Date(fechaActualMilisec);
        fechaDesdeFc.setValidators(Validators.compose([CustomValidators.maxDate(maxDate), Validators.required]));
        fechaDesdeFc.updateValueAndValidity();
      }
    });
    this.form = new FormGroup({
      fechaDesde: fechaDesdeFc,
      fechaHasta: fechaHastaFc,
      radioTipoReporte: radioReporte
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

  public checkInformes(id: number, event) {
    if (!this.informesAImprimir.some((Id) => Id === id) && event.target.checked) {
      this.informesAImprimir.push(id);
    }
    if (this.informesAImprimir.some((Id) => Id === id) && !event.target.checked) {
      let i = this.informesAImprimir.indexOf(id);
      this.informesAImprimir.splice(i, 1);
    }
  }
}
