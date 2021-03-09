export class BandejaMontoDisponibleConsulta {
  public fechaDesde: Date;
  public fechaHasta: Date
  public nroMonto: number;
  public incluirBaja: boolean;
  public numeroPagina: number;
  public tamañoPagina: number;

  constructor(fechaDesde?: Date,
              fechaHasta?: Date,
              nroMonto?: number,
              incluirBaja?: boolean,
              numeroPagina?: number,
              tamañoPagina?: number) {
    this.fechaDesde = fechaDesde;
    this.fechaHasta = fechaHasta;
    this.nroMonto = nroMonto;
    this.incluirBaja = incluirBaja;
    this.numeroPagina = numeroPagina;
    this.tamañoPagina = tamañoPagina;
  }
}
