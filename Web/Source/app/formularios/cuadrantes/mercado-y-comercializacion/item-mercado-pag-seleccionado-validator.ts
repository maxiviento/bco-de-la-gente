import { AbstractControl, FormArray } from '@angular/forms';

export const AlMenosUnItemSeleccionadoValidador = (control: AbstractControl): { [key: string]: any } => {
  let unoSeleccionado = false;
  const items = control.get('items') as FormArray;
  for (let item of items.controls) {
    if (item.value.seleccionado) {
      unoSeleccionado = true;
    }
  }

  if (unoSeleccionado) {
    return null;
  }
  return {noselected: true};
}
