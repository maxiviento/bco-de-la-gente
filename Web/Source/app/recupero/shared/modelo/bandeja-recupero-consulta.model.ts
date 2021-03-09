export class BandejaRecuperoConsulta {
  public fechaDesde: Date;
  public fechaHasta: Date;
  public idTipoEntidad: number;
  public numeroPagina: number;
  public tamañoPagina: number;

  constructor(fechaDesde?: Date,
              fechaHasta?: Date,
              idTipoEntidad?: number,
              numeroPagina?: number,
              tamañoPagina?: number) {
    this.fechaDesde = fechaDesde;
    this.fechaHasta = fechaHasta;
    this.idTipoEntidad = idTipoEntidad;
    this.numeroPagina = numeroPagina;
    this.tamañoPagina = tamañoPagina;
  }
}
