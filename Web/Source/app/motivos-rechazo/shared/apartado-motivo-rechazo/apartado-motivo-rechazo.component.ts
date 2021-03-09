import { Component, Input } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MotivoRechazo } from '../../../formularios/shared/modelo/motivo-rechazo';
import { Ambito } from '../modelo/ambito.model';
import { CustomValidators } from '../../../shared/forms/custom-validators';

@Component({
  selector: 'bg-apartado-motivo-rechazo',
  templateUrl: './apartado-motivo-rechazo.component.html',
  styleUrls: ['./apartado-motivo-rechazo.component.scss'],
})

export class ApartadoMotivoRechazoComponent {
  @Input('ambitos') public ambitos: Ambito[];
  @Input('motivo') public form: FormGroup;
  @Input() public habilitado: boolean;
  @Input() public edicion: boolean;

  public static nuevoFormGroup(motivo: MotivoRechazo = new MotivoRechazo()): FormGroup {
    if (motivo.esAutomatico == null || motivo.esAutomatico == undefined) {
      motivo.esAutomatico = false;
    }
    return new FormGroup({
      id: new FormControl(motivo.id),
      codigo: new FormControl(motivo.codigo, Validators.compose([
        Validators.maxLength(10),
        CustomValidators.validTextAndNumbers
      ])),
      nombre: new FormControl(motivo.nombre,
        Validators.compose([
          Validators.required,
          Validators.maxLength(100),
          CustomValidators.validTextAndNumbers])),
      descripcion: new FormControl(motivo.descripcion,
        Validators.compose([
          Validators.required,
          Validators.maxLength(200),
          CustomValidators.validTextAndNumbers])),
      abreviatura: new FormControl(motivo.abreviatura,
        Validators.compose([
          Validators.maxLength(30),
          Validators.minLength(2),
          CustomValidators.validTextAndNumbers])),
      ambito: new FormControl(motivo.ambito.id, Validators.compose([Validators.required, CustomValidators.minValue(1)]))
    });
  }

  public static prepararForm(form: FormGroup): MotivoRechazo {
    let motivo = new MotivoRechazo();
    motivo.ambito = new Ambito();
    motivo.id = form.value.id;
    motivo.codigo = form.value.codigo;
    motivo.nombre = form.value.nombre;
    motivo.descripcion = form.value.descripcion;
    motivo.abreviatura = form.value.abreviatura;
    motivo.esAutomatico = false;
    motivo.ambito.id = form.value.ambito;
    return motivo;
  }

  public deshabilitarAmbito(): boolean {
    if (!this.edicion) {
      return !this.habilitado ? true : null;
    } else {
      return this.edicion;
    }
  }
}
