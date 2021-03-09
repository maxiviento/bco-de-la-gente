import { AbstractControl, FormArray } from '@angular/forms';

export const AlMenosUnoSeleccionadoValidador = (control: AbstractControl): {[key: string]: any} => {
  let unoSeleccionado = false;
  const destinos = control.get('destinos') as FormArray;
  for (let destino of destinos.controls) {
    let dest = destino.value;
    if (dest.seleccionado) {
      unoSeleccionado = true;
      if (dest.id == 99) {
        let detalle = control.get('detalles');
        if (!detalle.value) {
          return {nodetail: true};
        }
      }
    }
  }
  if (unoSeleccionado) {
    return null;
  }
  return { noselected: true };
};

