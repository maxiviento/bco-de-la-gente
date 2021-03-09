import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { PagosService } from '../shared/pagos.service';
import { NgbUtils } from '../../shared/ngb/ngb-utils';
import { ModalidadPagoConsulta } from '../shared/modelo/modalidad-pago-consulta.model';
import { LoteCombo } from '../shared/modelo/lote-combo.model';
import { CustomValidators } from '../../shared/forms/custom-validators';
import { NgbDatepickerConfig } from '@ng-bootstrap/ng-bootstrap';
import { DateUtils } from '../../shared/date-utils';
import { Convenio } from '../../shared/modelo/convenio-model';

@Component({
  selector: 'bg-modalidad-pago',
  templateUrl: 'modalidad-pago.component.html',
  styleUrls: ['modalidad-pago.component.scss']
})
export class ModalidadPagoComponent implements OnInit {
  public fechaMinimaParaSimulacionPago: any;
  public formModalidad: FormGroup;
  public modalidades: LoteCombo[] = [];
  public elementos: LoteCombo[] = [];
  public convenios: Convenio[] = [];
  public consulta: ModalidadPagoConsulta;
  public fechaInicio: Date;
  public modalidad: string;
  public elemento: string;
  public convenio: number;
  public fechaFin: Date;
  public modificoMesGracia: boolean = false;
  public fechaFinPagoInicial: Date;
  public loteValido: boolean = true;
  @Input() public idLote: number;
  @Input() public loteConfirmado: boolean = false;
  @Input() public creacionLote: boolean = false;
  @Input() public esModal: boolean = false;

  constructor(private fb: FormBuilder,
              private pagosService: PagosService,
              private configFechas: NgbDatepickerConfig) {
    if (!this.consulta) {
      this.consulta = new ModalidadPagoConsulta();
    }
  }

  public ngOnInit(): void {
    DateUtils.removeMaxDateDP(this.configFechas);
    this.crearForm();
    this.pagosService.obtenerModalidadesPago().subscribe((modalidades) => {
      modalidades.forEach((m) => this.modalidades.push(m));
      this.pagosService.obtenerElementosPago().subscribe((elementos) => {
        elementos.forEach((e) => this.elementos.push(e));
        this.pagosService.obtenerConvenios().subscribe((convenios) => {
          convenios.forEach((c) => this.convenios.push(c));
          if (this.idLote) {
            this.pagosService.obtenerFechas(this.idLote).subscribe((res) => {
                this.fechaInicio = res.fecInicioPago;
                this.fechaFin = res.fecFinPago;
                this.fechaFinPagoInicial = res.fecFinPago;
                this.modalidad = res.modPago;
                this.elemento = res.elementoPago;
                this.convenio = res.convenioPago;
                this.crearForm();
              },
              (error) => {
                this.crearForm();
              });
          }
          this.formModalidad = this.fb.group({
            fechaPago: null,
            fechaFinPago: null,
            modalidad: null,
            elemento: null,
            convenio: null,
            mesesGracia: null,
            modificaMesGracia: false,
            mesesGraciaModificacion: null
          });
        });
      });
    });
    if (this.idLote) {
      this.pagosService.validarFormulariosLote(this.idLote).subscribe((res) => {
        if (!res) {
          this.loteValido = false;
        }
      });
    }
  }

  public crearForm() {
    let fechaInicio = new FormControl(NgbUtils.obtenerNgbDateStruct(this.fechaInicio));
    let fechaFin = new FormControl(NgbUtils.obtenerNgbDateStruct(this.fechaFin),
      CustomValidators.minDate(this.fechaInicio));
    fechaInicio.valueChanges.debounceTime(500).subscribe((value) => {
      if (NgbUtils.obtenerDate(value)) {
        fechaFin.clearValidators();
        let fechaDesdeMilisec = Date.parse(NgbUtils.obtenerDate(value).toISOString());
        let fechaActualMilisec = Date.parse(new Date().toISOString());
        let minDate;
        fechaDesdeMilisec <= fechaActualMilisec ? minDate = new Date(fechaDesdeMilisec) : minDate = new Date(fechaDesdeMilisec);
        fechaFin.setValidators(Validators.compose([CustomValidators.minDate(minDate), Validators.required]));
        fechaFin.updateValueAndValidity();
      }
    });
    fechaFin.valueChanges.debounceTime(500).subscribe((value) => {
      if (NgbUtils.obtenerDate(value)) {
        fechaInicio.clearValidators();
        let fechaHastaMilisec = Date.parse(NgbUtils.obtenerDate(value).toISOString());
        let fechaActualMilisec = Date.parse(new Date().toISOString());
        let maxDate;
        fechaHastaMilisec <= fechaActualMilisec ? maxDate = new Date(fechaHastaMilisec) : maxDate = new Date(fechaHastaMilisec);
        fechaInicio.setValidators([CustomValidators.maxDate(maxDate), Validators.required]);
        fechaInicio.updateValueAndValidity();
      }
    });
    let modificaMesGraciaFc = new FormControl(this.modificoMesGracia);
    modificaMesGraciaFc.valueChanges.subscribe((value) => {
      if (!value) {
        this.formModalidad.get('mesesGraciaModificacion').setValue(0);
        modificaMesGraciaFc.setValue('');
      }
    });

    this.consulta.modalidadId = this.modalidades.find((p) => p.descripcion == this.modalidad) ? this.modalidades.find((p) => p.descripcion == this.modalidad).id : null;
    this.consulta.elementoId = this.elementos.find((p) => p.descripcion == this.elemento) ? this.elementos.find((p) => p.descripcion == this.elemento).id : null;
    this.consulta.convenioId = this.convenios.find((p) => p.id == this.convenio) ? this.convenios.find((p) => p.id == this.convenio).id : null;
    this.formModalidad = this.fb.group({
      fechaPago: fechaInicio,
      fechaFinPago: fechaFin,
      modalidad: new FormControl(this.consulta.modalidadId, Validators.required),
      elemento: new FormControl(this.consulta.elementoId, Validators.required),
      convenio: new FormControl(this.consulta.convenioId, Validators.required),
      modificaMesGracia: modificaMesGraciaFc,
      mesesGraciaModificacion: new FormControl(this.consulta.mesesGracia,
        Validators.compose([CustomValidators.number, Validators.maxLength(2)])),
      mesesGracia: new FormControl(this.consulta.mesesGracia,
        Validators.compose([CustomValidators.number, Validators.maxLength(2)]))
    });

    this.fechaMinimaParaSimulacionPago = NgbUtils.obtenerNgbDateStruct(new Date());
    if (this.creacionLote) {
      this.formModalidad.get('mesesGracia').setValidators((Validators.compose([Validators.required, CustomValidators.number, Validators.min(0), Validators.maxLength(2)])));
    }
  }

  public obtenerFechaPago(): Date {
    return NgbUtils.obtenerDate(this.formModalidad.value.fechaPago);
  }

  public obtenerFechaFinPago(): Date {
    return NgbUtils.obtenerDate(this.formModalidad.value.fechaFinPago);
  }

  public obtenerModalidad(): number {
    return this.formModalidad.value.modalidad;
  }

  public obtenerElemento(): number {
    return this.formModalidad.value.elemento;
  }

  public obtenerConvenio(): number {
    return this.formModalidad.value.convenio;
  }

  public obtenerMesesGracia(): number {
    return this.formModalidad.value.mesesGracia;
  }

  public obtenerMesGraciaModificado(): number {
    if (this.formModalidad.value.mesesGraciaModificacion) {
      return this.formModalidad.value.mesesGraciaModificacion;
    }
    return -1;
  }

  public disableForm() {
    this.formModalidad.get('fechaPago').disable({emitEvent: false});
    this.formModalidad.get('fechaFinPago').disable({emitEvent: false});
    this.formModalidad.get('modalidad').disable({emitEvent: false});
    this.formModalidad.get('elemento').disable({emitEvent: false});
    this.formModalidad.get('convenio').disable({emitEvent: false});
    this.formModalidad.get('mesesGracia').disable({emitEvent: false});
    this.formModalidad.get('modificaMesGracia').disable({emitEvent: false});
    this.formModalidad.get('mesesGraciaModificacion').disable({emitEvent: false});
  }

  public esValido(): boolean {
    return this.formModalidad.valid &&
      this.formModalidad.get('fechaPago').value !== null &&
      this.formModalidad.get('fechaFinPago').value !== null;
  }

  public actualizaPlan(): boolean {
    return this.fechaFinPagoInicial.setHours(0) != NgbUtils.obtenerDate(this.formModalidad.value.fechaFinPago).setHours(0) || this.formModalidad.get('modificaMesGracia').value;
  }
}
