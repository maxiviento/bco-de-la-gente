export class LoteCombo {
  public id: number;
  public nombre: string;
  public descripcion: string;

  constructor(id?: number,
              descripcion?: string,
              nombre?: string) {
    this.id = id;
    this.nombre = nombre;
    this.descripcion = descripcion;
  }
}
