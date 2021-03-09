export class ConsultaInformePagos {
  public idsInformes: number[];
  public fechaDesde: Date;
  public fechaHasta: Date;

  constructor(idsInformes: number[],
              fechaDesde: Date,
              fechaHasta: Date) {
    this.idsInformes = idsInformes;
    this.fechaDesde = fechaDesde;
    this.fechaHasta = fechaHasta;
  }
}
