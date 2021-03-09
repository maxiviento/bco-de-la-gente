export class ItemsSeleccionados {
  public items: number[];
  public descripcion: string;
  public nombreTipo: string;

  constructor(items?: number[],
              descripcion?: string,
              nombreTipo?: string) {
    this.items = items;
    this.descripcion = descripcion;
    this.nombreTipo = nombreTipo;
  }
}
