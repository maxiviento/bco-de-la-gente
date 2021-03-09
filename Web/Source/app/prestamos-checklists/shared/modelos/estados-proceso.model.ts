export class EstadoProceso {
  public clave: number;
  public valor: string;

  constructor(id?: number, descripcion?: string) {
    this.clave = id;
    this.valor = descripcion;
  }
}
