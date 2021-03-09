export class BandejaSuafConsulta {
  public fechaDesde: Date;
  public fechaHasta: Date;
  public idLote: number;
  public numeroPagina: number;
  public tamañoPagina: number;

  constructor(fechaDesde?: Date,
              fechaHasta?: Date,
              idLote?: number) {
    this.fechaDesde = fechaDesde;
    this.fechaHasta = fechaHasta;
    this.idLote = idLote;
  }
}
