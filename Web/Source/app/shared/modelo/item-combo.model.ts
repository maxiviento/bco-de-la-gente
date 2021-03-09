export class ItemCombo {
  public clave: string;
  public valor: string;

  constructor(id?: string, descripcion?: string) {
    this.clave = id;
    this.valor = descripcion;
  }
}
