import { Component, Input } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Requisito } from '../../modelo/requisito-linea';
import { Item } from '../../../../../../items/shared/modelo/item.model';

@Component({
  selector: 'bg-requisito',
  templateUrl: 'requisito.component.html',
  styleUrls: ['requisito.component.scss']
})
export class RequisitoComponent {
  @Input('requisito') public form: FormGroup;
  @Input() public primero: boolean;
  @Input() public ultimo: boolean;

  public static nuevoFormGroup(item: Item): FormGroup {
    return new FormGroup({
      id: new FormControl(item.id),
      nombre: new FormControl(item.nombre),
      seleccionado: new FormControl(item.esSeleccionado),
    });
  }
}
