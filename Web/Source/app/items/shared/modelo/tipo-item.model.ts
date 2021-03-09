export class TipoItem {
  public id: number;
  public nombre: any;
  public esSeleccionado: boolean;

  constructor(id?: number, nombre?: any, esSeleccioado?: boolean) {
    this.id = id;
    this.nombre = nombre;
    this.esSeleccionado = esSeleccioado;
  }
}
