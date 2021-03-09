import { Input, Component } from '@angular/core';
import { FormGroup, Validators, FormControl } from '@angular/forms';
import { MotivoDestino } from '../modelo/motivo-destino.model';
import { CustomValidators } from '../../../shared/forms/custom-validators';

@Component({
  selector: 'bg-apartado-motivo-destino',
  templateUrl: './apartado-motivo-destino.component.html',
  styleUrls: ['./apartado-motivo-destino.component.scss'],
})

export class ApartadoMotivoDestinoComponent {
  @Input('motivo') public form: FormGroup;
  @Input() public editable: boolean;

  public static nuevoFormGroup(motivo: MotivoDestino = new MotivoDestino()): FormGroup {
    return new FormGroup({
      id: new FormControl(motivo.id),
      nombre: new FormControl(motivo.nombre,
        Validators.compose([
          Validators.required,
          Validators.maxLength(100),
          CustomValidators.validTextAndNumbers])),
      descripcion: new FormControl(motivo.descripcion,
        Validators.compose([
          Validators.required,
          Validators.maxLength(200),
        CustomValidators.validTextAndNumbers]))
    });
  }

  public static prepararForm(form: FormGroup): MotivoDestino {
    let motivo = new MotivoDestino();
    motivo.id = form.value.id;
    motivo.nombre = form.value.nombre;
    motivo.descripcion = form.value.descripcion;
    return motivo;
  }
}
