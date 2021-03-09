import { Component, Input } from '@angular/core';
import { FormArray, FormControl, FormGroup } from '@angular/forms';
import { RequisitoComponent } from './requisito/requisito.component';
import { ItemTipoItem } from '../../../../../items/shared/modelo/item-tipo-item';

@Component({
  selector: 'bg-tipo-requisito',
  templateUrl: 'tipo-requisito.component.html',
  styleUrls: ['tipo-requisito.component.scss']
})
export class TipoRequisitoComponent {
  @Input('tipoRequisito') public form: FormGroup;

  public static nuevoFormGroup(tipo: ItemTipoItem): FormGroup {
    return new FormGroup({
      id: new FormControl(tipo.id),
      nombre: new FormControl(tipo.nombre),
      requisitos: new FormArray((tipo.items || [])
        .map((item) => RequisitoComponent.nuevoFormGroup(item)))
    });
  }

  public get requisitos(): FormArray {
    return this.form.get('requisitos') as FormArray;
  }
}
