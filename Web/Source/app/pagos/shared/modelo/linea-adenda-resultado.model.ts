export class LineaAdendaResultado {
  public nombreLinea: string;
  public idLinea: number;
  public limiteCuota: number;

  constructor(nombreLinea?: string,
              idLinea?: number,
              limiteCuota?: number) {
    this.nombreLinea = nombreLinea;
    this.idLinea = idLinea;
    this.limiteCuota = limiteCuota;
  }
}
