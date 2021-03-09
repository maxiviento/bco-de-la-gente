export class LineaCombo {
  public id: number;
  public descripcion: string;
  public dadoDeBaja: boolean;

  constructor(id?: number,
              descripcion?: string,
              dadoDeBaja?: boolean) {
    this.id = id;
    this.descripcion = descripcion;
    this.dadoDeBaja = dadoDeBaja;
  }
}
