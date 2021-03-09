import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MotivoRechazo } from '../shared/modelo/motivo-rechazo';
import { MotivoRechazoService } from '../shared/motivo-rechazo.service';
import { MotivoRechazoComando } from '../shared/modelo/motivo-rechazo-comando.model';
import { CustomValidators } from '../../shared/forms/custom-validators';

@Component({
  selector: 'bg-modal-motivo-rechazo',
  templateUrl: './modal-motivo-rechazo.component.html',
  styleUrls: ['./modal-motivo-rechazo.component.scss'],
})

export class ModalMotivoRechazoComponent implements OnInit {
  public form: FormGroup;
  public motivosRechazo: MotivoRechazo [] = [];
  public idsMotivoRechazo: MotivoRechazo[] = [];
  public numeroCaja: string;
  public modalMultiplesObservaciones: boolean = false;
  @Input() public ambito: string;
  @Input() public unSoloMotivo: boolean = false;
  @Input() public muestraObservaciones: boolean = true;
  @Input() public lsMotivosRechazoAnteriores: MotivoRechazo [] = [];
  @Input() public nroCaja: string;

  constructor(private fb: FormBuilder,
              private motivoRechazoService: MotivoRechazoService,
              private activeModal: NgbActiveModal) {
  }

  public ngOnInit(): void {
    if (this.ambito === 'Formulario' || this.ambito === 'Prestamo' || this.ambito === 'Checklist') {
      this.modalMultiplesObservaciones = true;
    }

    if (this.lsMotivosRechazoAnteriores.length) {
      this.idsMotivoRechazo.push(...this.lsMotivosRechazoAnteriores);
    }

    if (this.nroCaja) {
      this.numeroCaja = this.nroCaja;
    }
    this.crearForm();

    this.motivoRechazoService.consultarMotivoRechazo(this.ambito)
      .subscribe((motivosRechazo) => {
        this.motivosRechazo = motivosRechazo;
        this.concatenarCodigoDescripcion();
      });
  }

  private crearForm(): void {
    this.form = this.fb.group({
      motivoRechazoSeleccionado: ['', Validators.required],
      observaciones: ['', Validators.maxLength(500)],
      numeroCaja: ['', Validators.maxLength(9)]
    });
    if (this.modalMultiplesObservaciones) {
      this.form.get('numeroCaja').setValidators([CustomValidators.validTextAndNumbers, Validators.required, Validators.maxLength(9)]);
      if (this.nroCaja) {
        this.form.get('numeroCaja').setValue(this.nroCaja);
      }
    }
  }

  public rechazar(): void {
    this.activeModal.close(this.prepararRechazo());
  }

  public agregarMotivo() {
    if (this.idsMotivoRechazo.find((motivo) => motivo.id === this.motivoSeleccionado.value)) {
      return null;
    }
    let motivo = this.motivosRechazo.find((m) => m.id === this.motivoSeleccionado.value);
    if (motivo) {
      if (this.modalMultiplesObservaciones) {
        motivo.observaciones = this.form.get('observaciones').value;
        this.idsMotivoRechazo.push(motivo);
        this.form.get('observaciones').setValue(null, {emitEvent: false});
      } else {
        this.idsMotivoRechazo.push(motivo);
      }
      this.form.get('motivoRechazoSeleccionado').setValue(null, {emitEvent: false});
    }
  }

  public quitarMotivo(idMotivo: number) {
    const index = this.idsMotivoRechazo.findIndex((motivo) => idMotivo === motivo.id);
    this.idsMotivoRechazo.splice(index, 1);
  }

  private prepararRechazo(): MotivoRechazoComando {
    let form = this.form.value;
    if (this.modalMultiplesObservaciones) {
      form.observaciones = null;
    }
    if (this.unSoloMotivo) {
      this.idsMotivoRechazo.push(this.motivosRechazo.find((m) => m.id === this.motivoSeleccionado.value));
    }
    return new MotivoRechazoComando(this.idsMotivoRechazo, form.numeroCaja, form.observaciones);
  }

  public get motivoSeleccionado(): FormControl {
    return this.form.get('motivoRechazoSeleccionado') as FormControl;
  }

  public cerrar(): void {
    this.activeModal.close();
  }

  public inhablitarRechazo(): boolean {
    if (this.unSoloMotivo && this.form.get('motivoRechazoSeleccionado').value) {
      return null;
    }
    if (this.form.get('numeroCaja').valid && this.modalMultiplesObservaciones && this.idsMotivoRechazo.length) {
      return null;
    }
    return true;
  }

  public concatenarCodigoDescripcion() {
    this.motivosRechazo.forEach((motivo) => {
      if (motivo.codigo) {
        motivo.descripcion = motivo.codigo + ' - ' + motivo.nombre;
      }
    });
  }
}
