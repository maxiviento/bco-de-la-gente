import { Pipe, PipeTransform } from '@angular/core';
import { CurrencyPipe } from '@angular/common';

@Pipe({name: 'moneda'})
export class MonedaPipe implements PipeTransform {
  constructor(private currencyPipe: CurrencyPipe) {
  }

  transform(value: any, decimal: number = 2): string {
    if (value == null)
      return '';

    decimal = decimal < 0 ? 2: decimal;
    let format = `1.${decimal}-${decimal}`;
    return this.currencyPipe.transform(value, 'ARS', true, format);

  }
}
