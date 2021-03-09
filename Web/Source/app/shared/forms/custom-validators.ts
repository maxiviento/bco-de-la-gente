import { AbstractControl, ValidationErrors, ValidatorFn, FormGroup, FormArray, FormControl } from '@angular/forms';
import { isNullOrUndefined } from 'util';
import { isString } from 'util';
import { isNull } from 'util';
import { NgbUtils } from '../ngb/ngb-utils';

const NUMBER_REGEXP = /^\d+$/;
const EMAIL_REGEXP = /^(?=.{1,254}$)(?=.{1,64}@)[-!#$%&'*+/0-9=?A-Z^_`a-z{|}~]+(\.[-!#$%&'*+/0-9=?A-Z^_`a-z{|}~]+)*@[A-Za-z0-9]([A-Za-z0-9-]{0,61}[A-Za-z0-9])?(\.[A-Za-z0-9]([A-Za-z0-9-]{0,61}[A-Za-z0-9])?)*$/;
const DECIMAL_REGEXP = /^-?\d+(\.?\d+)?$/;
const DEVENGADO_REGEXP = /^(\d{4}[\/\-]\d{6})$/;
const DEVENGADO_NULO_REGEXP = /(0{4}\/0{6})/;
const DECIMAL_WITH_TWO_DIGITS_REGEXP = /^[0-9]+([.][0-9]{0,2})?$/;
const TEXTO_REGEXP = /[^a-zA-Z]/g;
const TEXTO_NUMEROS_REGEXP = /[^0-9a-zA-Z]/g;
const FILENAME_REGEXP = /^([a-zA-Z_\-\d])+$/;

export function isEmpty(value: any): boolean {
  return isNullOrUndefined(value)
    || (isString(value) && value === '');
}

export class CustomValidators {

  public static codArea(control: AbstractControl): ValidationErrors | null {
    if (isEmpty(control.value)) {
      return null;
    }
    return (control.value > 9) ? null : {codArea: true};
  }
  public static nroTelefono(control: AbstractControl): ValidationErrors | null {
    if (isEmpty(control.value)) {
      return null;
    }
    return (control.value > 9) ? null : {telefono: true};
  }

  public static number(control: AbstractControl): ValidationErrors | null {
    if (isEmpty(control.value)) {
      return null;
    }
    return NUMBER_REGEXP.test(control.value) ? null : {number: true};
  }

  public static decimalNumber(control: AbstractControl): ValidationErrors | null {
    if (isEmpty(control.value)) {
      return null;
    }
    return DECIMAL_REGEXP.test(control.value) ? null : {number: true};
  }

  public static decimalNumberWithTwoDigits(control: AbstractControl): ValidationErrors | null {
    if (isEmpty(control.value)) {
      return null;
    }
    return DECIMAL_WITH_TWO_DIGITS_REGEXP.test(control.value) ? null : {'twodecimal': true};
  }

  public static date(control: AbstractControl): ValidationErrors | null {
    const date = NgbUtils.obtenerDate(control.value);
    if (isEmpty(date)) {
      return null;
    }
    return (date instanceof Date && (/(\d{4})/).test(date.getFullYear().toString())) ? null : {date: true};
  }

  public static email(control: AbstractControl): ValidationErrors | null {
    if (isEmpty(control.value)) {
      return null;
    }
    return EMAIL_REGEXP.test(control.value) ? null : {email: true};
  }

  public static validText(control: AbstractControl): ValidationErrors | null {
    if (isEmpty(control.value)) {
      return null;
    }
    return (control.value.toString().replace(TEXTO_REGEXP, '').length !== 0) ? null : {validText: true};
  }

  public static validTextAndNumbers(control: AbstractControl): ValidationErrors | null {
    if (isEmpty(control.value)) {
      return null;
    }
    return (control.value.toString().replace(TEXTO_NUMEROS_REGEXP, '').length !== 0) ? null : {validTextAndNumbers: true};
  }

  public static fileName(control: AbstractControl): ValidationErrors | null {
    if (isEmpty(control.value)) {
      return null;
    }
    return FILENAME_REGEXP.test(control.value) ? null : {fileName: true};
  }

  public static minValue(minValue: number): ValidatorFn {

    return (control) => {
      let numero = CustomValidators.number(control);

      if (!isNull(numero)) {
        return numero;
      }

      if (isEmpty(control.value)) {
        return null;
      }

      const value = parseInt(control.value, 10);

      return isNaN(value) || (value < minValue) ?
        {minvalue: {requiredValue: minValue, actualValue: control.value}} :
        null;
    };
  }

  public static maxValue(maxValue: number) {
    return (control) => {

      let numero = CustomValidators.number(control);

      if (!isNull(numero)) {
        return numero;
      }

      if (isEmpty(control.value)) {
        return null;
      }

      const value = parseInt(control.value, 10);

      return isNaN(value) || (value > maxValue) ?
        {maxvalue: {requiredValue: maxValue, actualValue: control.value}} :
        null;
    };
  }

  public static minDecimalValue(minValue: number): ValidatorFn {

    return (control) => {
      let numero = CustomValidators.decimalNumber(control);

      if (!isNull(numero)) {
        return numero;
      }

      if (isEmpty(control.value)) {
        return null;
      }

      const value = parseFloat(control.value);

      return isNaN(value) || (value < minValue) ?
        {minvalue: {requiredValue: minValue, actualValue: control.value}} :
        null;
    };
  }

  public static maxDecimalValue(maxValue: number) {
    return (control) => {

      let numero = CustomValidators.decimalNumber(control);

      if (!isNull(numero)) {
        return numero;
      }

      if (isEmpty(control.value)) {
        return null;
      }

      const value = parseFloat(control.value);

      return isNaN(value) || (value > maxValue) ?
        {maxvalue: {requiredValue: maxValue, actualValue: control.value}} :
        null;
    };
  }

  public static minDate(minDate: Date): ValidatorFn {
    return (control) => {
      let date = CustomValidators.date(control);
      if (!isNull(date)) {
        return date;
      }
      if (isEmpty(control.value)) {
        return null;
      }
      const value = NgbUtils.obtenerDate(control.value);
      return (value < minDate) ?
        {minDate: {requiredValue: NgbUtils.obtenerStringDate(minDate), actualValue: control.value}}
        : null;
    };
  }

  public static maxDate(maxDate: Date): ValidatorFn {
    return (control) => {
      let date = CustomValidators.date(control);
      if (!isNull(date)) {
        return date;
      }
      if (isEmpty(control.value)) {
        return null;
      }
      const value = NgbUtils.obtenerDate(control.value);
      return (value > maxDate) ?
        {maxDate: {requiredValue: NgbUtils.obtenerStringDate(maxDate), actualValue: control.value}}
        : null;
    };
  }

  public static distintoA(valor: any): ValidatorFn {

    return (control) => {

      if (isEmpty(control.value)) {
        return null;
      }

      let valorActual = control.value;

      if (!isNaN(valor)) {
        valorActual = parseFloat(valorActual);
      }

      return (valorActual !== valor) ?
        null :
        {distintoA: {requiredValue: valor, actualValue: control.value}};
    };
  }

  public static cantidadDigitos(digitos: number): ValidatorFn {

    return (control) => {
      let numero = CustomValidators.number(control);

      if (!isNull(numero)) {
        return numero;
      }

      if (isEmpty(control.value)) {
        return null;
      }

      const valorActual = control.value;
      return (valorActual.toString().length === digitos) ?
        null :
        {cantidadDigitos: {requiredValue: digitos, actualValue: control.value}};
    };
  }

  public static soloUnCampo(controlsAVerificar?: string[]): ValidatorFn {
    return (group) => {
      let tieneValor: number = 0;
      if (group && group instanceof FormGroup && group.controls) {
        for (let control in controlsAVerificar) {
          if (group.controls.hasOwnProperty(control) && group.controls[control].valid
            && group.controls[control].value) {
            tieneValor++;
          }
        }
      }
      return tieneValor <= 1 ? null : {soloUnCampo: {requiredValue: true, actualValue: tieneValor}};
    };
  }

  public static alMenosUnCampo(group: AbstractControl, controlAVerificar?: string): ValidationErrors | null {
    let isAtLeastOne = false;
    if (group && group instanceof FormGroup && group.controls) {
      if (group.controls.hasOwnProperty(controlAVerificar) && group.controls[controlAVerificar].valid
        && group.controls[controlAVerificar].value) {
        isAtLeastOne = true;
      }
    }
    return isAtLeastOne ? null : {alMenosUnCampo: true};
  }

  public static alMenosUnItem(controlAVerificar?: string, nombreItem?: string): ValidatorFn {
    return (array) => {
      let alMenosUnItem: boolean = false;
      if (array && array instanceof FormArray && array.controls) {
        for (let group in array.controls) {
          let tieneAlMenosUnCampo = CustomValidators.alMenosUnCampo(array.controls[group], controlAVerificar);
          if (tieneAlMenosUnCampo === null) {
            alMenosUnItem = true;
            break;
          }
        }
      }
      return alMenosUnItem ? null : {
        alMenosUnItem: {
          requiredValue: true,
          actualValue: alMenosUnItem,
          item: nombreItem
        }
      };
    };
  }

  public static tieneFiltros(group: FormGroup): ValidationErrors {
    let tieneFiltro = false;
    if (group && group instanceof FormGroup && group.controls &&
      Object.getOwnPropertyNames(group.controls).some((property) =>
      group.controls[property].value && group.controls[property].valid)) {
      tieneFiltro = true;
    }
    return tieneFiltro ? {tieneFiltros: true} : null;
  }

  public static devengado(control: AbstractControl): ValidationErrors | null {
    if (isEmpty(control.value)) {
      return null;
    }
    if (DEVENGADO_NULO_REGEXP.test(control.value)) {
        return {devengadoNulo: true}
    }
    return DEVENGADO_REGEXP.test(control.value) ? null : {number: true};
  }
}
export class ErrorMessages {
  public static messageOf(validatorName: string, validatorValue?: any) {
    let config = {
      codArea: 'Debe ingresar un código válido',
      telefono: 'Debe ingresar un número válido',
      validText: 'Debe ingresar un texto válido',
      validTextAndNumbers: 'Debe ingresar un texto válido',
      required: 'El campo es requerido.',
      number: 'Ingresar sólo números.',
      email: 'El email es inválido.',
      maxlength: `No superar los ${validatorValue.requiredLength} caracteres.`,
      minlength: `Ingresar al menos ${validatorValue.requiredLength} caracteres.`,
      minvalue: `El valor mínimo es ${validatorValue.requiredValue}.`,
      maxvalue: `El valor máximo es ${validatorValue.requiredValue}.`,
      minDate: `La fecha mínima es ${validatorValue.requiredValue }.`,
      maxDate: `La fecha máxima es ${validatorValue.requiredValue }.`,
      minDecimalValue: `El valor mínimo es ${validatorValue.requiredValue}.`,
      cantidadDigitos: `La cantidad de digitos requerida es ${validatorValue.requiredValue }.`,
      date: 'Ingresar sólo fechas.',
      alMenosUnCampo: 'Al menos un campo es requerido',
      alMenosUnItem: `Al menos un ${validatorValue.item } es requerido y debe ser marcado como entregado.`,
      soloUnCampo: 'Solo un campo es permitido',
      distintoA: `El valor debe ser distinto a ${validatorValue.requiredValue }.`,
      twodecimal: 'Solo se admiten números positivos y con dos decimales',
      fileName: 'Solo se admiten letras, guiones y números',
      devengadoNulo: 'El número es inválido'
    };
    return config[validatorName];
  }
}
