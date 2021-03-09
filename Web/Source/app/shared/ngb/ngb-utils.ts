import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { DateUtils } from '../date-utils';

export class NgbUtils {
  public static padDate = (num) => {
    let norm = Math.abs(Math.floor(num));
    return (norm < 10 ? '0' : '') + norm;
  }

  public static obtenerNgbDateStruct(fecha: Date): NgbDateStruct {
    if (!fecha) {
      return undefined;
    }
    return {day: fecha.getDate(), month: fecha.getMonth() + 1, year: fecha.getFullYear()};
  }

  public static obtenerDate(fecha: NgbDateStruct): Date {
    if (!fecha || !fecha.year) {
      return undefined;
    }
    fecha.year = parseInt(fecha.year.toString().slice(0, 4), 10);
    return new Date(
      fecha.year,
      fecha.month - 1,
      fecha.day);
  }

  public static obtenerStringDate(fecha?: Date): string {
    if (!fecha) {
      return undefined;
    }
    let anio = NgbUtils.padDate(fecha.getFullYear()).toString();
    let mes = NgbUtils.padDate((fecha.getMonth() + 1)).toString();
    let dia = NgbUtils.padDate(fecha.getDate()).toString();
    return dia + '/' + mes + '/' + anio;
  }

  public static calcularEdad(fecha?: Date): number {
    if (!fecha) {
      return undefined;
    }
    if (typeof fecha === 'string'
      && DateUtils.isISODate(fecha)) {
      fecha = DateUtils.convertToDate(fecha);
    }
    if (!(fecha instanceof Date)) {
      return undefined;
    }
    let hoy = new Date();
    let edad = hoy.getFullYear() - fecha.getFullYear();
    let m = hoy.getMonth() - fecha.getMonth();
    if (m < 0 || (m === 0 && hoy.getDate() < fecha.getDate())) {
      edad--;
    }
    return edad;
  }
}

