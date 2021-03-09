import { isDefined } from '@ng-bootstrap/ng-bootstrap/util/util';
import { NgbDatepickerConfig } from '@ng-bootstrap/ng-bootstrap';

export class DateUtils {

  private static REGEX_ISO: RegExp = /(\d{4})-(\d{2})-(\d{2})T(\d{2})\:(\d{2})\:(\d{2})/;

  public static convertToDate(fecha: string): Date {
    let match;
    if (DateUtils.isISODate(fecha)) {
      match = fecha.match(this.REGEX_ISO);
      let now = new Date();
      let tzo = -now.getTimezoneOffset();
      let dif = tzo >= 0 ? '+' : '-';
      let pad = (num) => {
          let norm = Math.abs(Math.floor(num));
          return (norm < 10 ? '0' : '') + norm;
        };

      let date = match[0];

      if (date.search(/[-+]([0-9][0-9]):([0-9][0-9])/i) == -1) {
        date += dif + pad(tzo / 60) + ':' + pad(tzo % 60);
      }
      let milliseconds = Date.parse(date);
      if (isDefined(milliseconds)) {
        return new Date(milliseconds);
      }
    }
  }

  public static isISODate(fecha: string): boolean {
    return (fecha && typeof fecha == 'string' && fecha.match(this.REGEX_ISO) != null);
  }

  public static getManianaDate(): Date {
    let maniana = new Date();
    maniana.setHours(0, 0, 0, 0);
    maniana.setDate(maniana.getDate() + 1);
    return this.initDateToBeginingOfDay(maniana);
  }

  public static initDateToBeginingOfDay(date: Date): Date {
    if (date instanceof Date) {
      date.setHours(0, 0, 0, 0); // resto hs del uso horario de Argentina para que quede bien la hora.
      return date;
    }
    return null;
  }

  public static setMinDateDP(date: Date, config: NgbDatepickerConfig): void {
    config.minDate = {
      year: date.getFullYear(),
      month: date.getMonth() + 1,
      day: date.getDate()
    };
  }

  public static setMaxDateDP(date: Date, config: NgbDatepickerConfig): void {
    config.maxDate = {
      year: date.getFullYear(),
      month: date.getMonth() + 1,
      day: date.getDate()
    };
  }

  public static removeMinDateDP(config: NgbDatepickerConfig): void {
    delete config.minDate;
  }

  public static removeMaxDateDP(config: NgbDatepickerConfig): void {
    delete config.maxDate;
  }

  /**
   * Compares two Date objects and returns e number value that represents
   * the result:
   * 0 if the two dates are equal.
   * 1 if the first date is greater than second.
   * -1 if the first date is less than second.
   * @param date1 First date object to compare.
   * @param date2 Second date object to compare.
   */
  public static compareDate(date1: Date, date2: Date): number
  {
    // With Date object we can compare dates them using the >, <, <= or >=.
    // The ==, !=, ===, and !== operators require to use date.getTime(),
    // so we need to create a new instance of Date with 'new Date()'
    let d1 = new Date(date1); let d2 = new Date(date2);

    // Check if the dates are equal
    let same = d1.getTime() === d2.getTime();
    if (same) return 0;

    // Check if the first is greater than second
    if (d1 > d2) return 1;

    // Check if the first is less than second
    if (d1 < d2) return -1;
  }
}
