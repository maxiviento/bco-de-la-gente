import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbUtils } from '../../shared/ngb/ngb-utils';
import { PrestamoService } from '../../shared/servicios/prestamo.service';
import { ProvidenciaComando } from '../shared/modelo/providencia-comando.model';
import { CustomValidators } from '../../shared/forms/custom-validators';

@Component({
  selector: 'bg-modal-fecha-providencia',
  templateUrl: './modal-fecha-providencia.component.html',
  styleUrls: ['./modal-fecha-providencia.component.scss'],
})

export class ModalFechaProvidenciaComponent implements OnInit {
  public form: FormGroup;
  public ingresoManual: boolean;
  public fechaAprobacion: Date;
  public fechaProvidencia: Date;
  public idPrestamo: number;
  public providenciaMasiva: boolean;
  public fechaActivacionMasiva: boolean;
  public providenciaComando = new ProvidenciaComando();
  @Input()
  public idFormulario: number;

  constructor(private prestamoService: PrestamoService,
              public activeModal: NgbActiveModal,
              private fb: FormBuilder) {
  }

  public ngOnInit(): void {
    this.ingresoManual = false;
    if (this.idFormulario === -1) {
      // Para providencia masiva
      this.providenciaMasiva = true;
      this.fechaActivacionMasiva = false;
      this.crearForm();
    } else {
      // Para providencia desde el checklist
      this.prestamoService.obtenerIdPrestamo(this.idFormulario).subscribe((res) => {
        this.idPrestamo = res;
        this.fechaActivacionMasiva = false;
        this.crearForm();
      });
    }
  }

  public crearForm() {
    let nuevaFechaFC = new FormControl(NgbUtils.obtenerNgbDateStruct(new Date(Date.now())));
    this.form = this.fb.group({fecha: nuevaFechaFC});
  }

  public ingresoManualFecha(): void {
    this.ingresoManual = true;
    let fechaManual = this.form.get('fecha') as FormControl;
    fechaManual.setValidators(Validators.compose([Validators.required, CustomValidators.minDate(new Date('01/01/1900'))]));
  }

  public utilizarFechaAprobacion(): void {
    if (this.providenciaMasiva) {
      // Para providencia masiva
      this.providenciaComando.fechaAprovacionMasiva = true;
      this.activeModal.close(this.providenciaComando);
    } else {
      // Para providencia desde el checklist
      this.prestamoService.obtenerFechaAprobacion(this.idPrestamo).subscribe((res) => {
        this.fechaAprobacion = res.fechAprobacion;
        this.fechaProvidencia = this.fechaAprobacion;
        this.activeModal.close(this.fechaProvidencia);
      });
    }
  }

  public confirmarFecha() {
    let formModel = this.form.value;
    this.fechaProvidencia = NgbUtils.obtenerDate(formModel.fecha);
    if (this.providenciaMasiva) {
      // Para providencia masiva
      this.providenciaComando.fechaAprovacionMasiva = false;
      this.providenciaComando.fecha = NgbUtils.obtenerDate(formModel.fecha);
      this.activeModal.close(this.providenciaComando);
    }
    this.activeModal.close(this.fechaProvidencia);
  }

  public cancelar(): void {
    this.activeModal.close();
  }
}
