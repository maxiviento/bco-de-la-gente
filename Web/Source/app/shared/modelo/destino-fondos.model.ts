export class DestinoFondos {
  public id: number;
  public descripcion: string;
  public detalle: string;
  public seleccionado: boolean;

  constructor(id?: number,
              descripcion?: string,
              detalle?: string,
              seleccionado?: boolean) {
    this.id = id;
    this.descripcion = descripcion;
    this.detalle = detalle;
    this.seleccionado = seleccionado;
  }
}
