export class VigenciaParametro {

  public valor: string;
  public id: number;
  public fechaDesde: Date;
  public fechaHasta: Date;

  constructor(valor?: string,
              id?: number,
              fechaDesde?: Date,
              fechaHasta?: Date) {

    this.valor = valor;
    this.id = id;
    this.fechaDesde = fechaDesde;
    this.fechaHasta = fechaHasta;
  }
}
