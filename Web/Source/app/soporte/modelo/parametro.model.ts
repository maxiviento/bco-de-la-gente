import { DateUtils } from '../../shared/date-utils';

export class Parametro {
  public static TIPO_NUMERICO = 4;
  public static TIPO_LOGICO = 3;
  public static TIPO_FECHA = 1;

  public id: number;
  public idVigencia: number;
  public nombre: string;
  public descripcion: string;
  public idTipoDato: number;
  public nombreTipoDato: string;
  public valor: string;
  public fechaDesde: Date;
  public fechaHasta: Date;
  public valorConfigurado: any;
  public vigente: boolean;

  constructor() {
    this.id = undefined;
    this.idVigencia = undefined;
    this.nombre = undefined;
    this.descripcion = undefined;
    this.idTipoDato = undefined;
    this.nombreTipoDato = undefined;
    this.valor = undefined;
    this.fechaDesde = undefined;
    this.fechaHasta = undefined;
    this.vigente = undefined;
  }

  public configurarValor(): void {
    this.valor = this.valor.trim();
    switch (this.idTipoDato) {
      case Parametro.TIPO_NUMERICO:
        this.valorConfigurado = this.valor && this.valor.length ? parseFloat(this.valor) as number : undefined;
        break;
      case Parametro.TIPO_LOGICO:
        this.valorConfigurado = this.valor && this.valor.length ? this.traerBooleanoString(this.valor): undefined;
        break;
      case Parametro.TIPO_FECHA:
        this.valorConfigurado = this.valor && this.valor.length ? DateUtils.convertToDate(this.valor) : undefined;
        break;
      default:
        this.valorConfigurado = this.valor as string;
    }
  }

  public clone(): Parametro {
    let clone = Object.assign(new Parametro(), this);
    clone.fechaDesde = new Date();
    return clone;
  }

  public configurarNuevaVigencia() {
    this.fechaDesde = DateUtils.getManianaDate();
  }

  public estaVigente(): boolean {
    let maniana = DateUtils.getManianaDate();
    return (this.fechaHasta && this.fechaHasta !== null);
  }

  public venceHoy(): boolean { //verificar en base a datos datos
    return this.fechaDesde.getDate() === DateUtils.getManianaDate().getDate();
  }

  public tieneFechaHasta(): boolean {
    return this.fechaHasta && this.fechaHasta !== null;
  }

  static obtenerBooleanDB(valor: string):string{
    return valor == 'VERDADERO' ? 'S' : 'N';
  }

  private traerBooleanoString(valor: string): string{
    return valor == 'S' ? 'VERDADERO' : 'FALSO';
}
}
