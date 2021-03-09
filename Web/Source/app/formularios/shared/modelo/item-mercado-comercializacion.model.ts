export class ItemMercadoComercializacion {
  public id: number;
  public idCategoria: number;
  public nombreTipo: string;
  public nombre: string;
  public descripcion: string;
  public seleccionado: boolean;

  constructor(id?: number,
              nombre?: string,
              idCategoria?: number,
              descripcion?: string,
              nombreTipo?: string) {
    this.id = id;
    this.idCategoria = idCategoria;
    this.nombreTipo = nombreTipo;
    this.nombre = nombre;
    this.descripcion = descripcion;
    this.seleccionado = false;
  }
}
