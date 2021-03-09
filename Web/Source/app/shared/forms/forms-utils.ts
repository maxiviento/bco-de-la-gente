import { AbstractControl, FormArray, FormControl, FormGroup } from '@angular/forms';

export class FormUtils {
  public static validate(control: AbstractControl): void {
    if (control instanceof FormControl) {
      control.markAsDirty();
      control.updateValueAndValidity();
    }
    if (control instanceof FormGroup) {
      Object.keys((<FormGroup> control).controls)
        .forEach((key) => {
          FormUtils.validate((<FormGroup> control).controls[key]);
        });
    }
    if (control instanceof FormArray) {
      (<FormArray> control).controls
        .forEach((childcontrol) => {
          FormUtils.validate(childcontrol);
        });
    }
  }

  public static verificarTipoDatoDate(control: FormControl) {
    control.valueChanges.distinctUntilChanged().subscribe((valor) => {
      if (typeof valor === 'string' && valor === '') {
        control.setValue(null);
      }
    });
  }
}
