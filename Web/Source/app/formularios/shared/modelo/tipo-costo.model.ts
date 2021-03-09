export class TipoCosto {
  public id: number;
  public idTipo: number;
  public nombre: string;

  constructor(id?: number,
              idTipo?: number,
              nombre?: string) {
    this.id = id;
    this.idTipo = idTipo;
    this.nombre = nombre;
  }
}
