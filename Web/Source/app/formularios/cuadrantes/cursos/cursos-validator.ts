import { AbstractControl, FormArray } from '@angular/forms';

export const AlMenosUnCursoSeleccionadoValidador = (control: AbstractControl): {[key: string]: any} => {
  let unoSeleccionado = false;
  const categorias = control.get('categorias') as FormArray;
  for (let categoria of categorias.controls) {
    const cursos = categoria.get('cursos') as FormArray;
    for (let curso of cursos.controls) {
      if (curso.value.seleccionado) {
        unoSeleccionado = true;
      }

      let otros = categoria.get('otros').value;
      if (otros.descripcion != '') {
        unoSeleccionado = true;
      }
    }
  }
  if (unoSeleccionado) {
    return null;
  }
  return { noselected: true };
};

