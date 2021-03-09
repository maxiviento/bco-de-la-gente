export class ItemMercadoComerSeleccionado {
  public items: number[];
  public descripcion: string;
  public tipoItem: number;

  constructor(items?: number[],
              descripcion?: string,
              tipoItem?: number) {
    this.items = items;
    this.descripcion = descripcion;
    this.tipoItem = tipoItem;
  }
}
