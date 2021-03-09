export class Costo {
  public id: number;
  public idTipo: number;
  public idItem: number;
  public nombre: string;
  public detalle: string;
  public precioUnitario: number;
  public valorMensual: number;

  constructor(id?: number,
              idTipo?: number,
              idItem?: number,
              detalle?: string,
              precioUnitario?: number,
              cantMensauales?: number,
              nombre?: string) {
    this.id = id;
    this.idTipo = idTipo;
    this.idItem = idItem;
    this.nombre = nombre;
    this.detalle = detalle;
    this.precioUnitario = precioUnitario;
    this.valorMensual = cantMensauales;
  }
}
