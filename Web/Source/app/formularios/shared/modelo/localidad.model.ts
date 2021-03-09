export class Localidad{
  public clave: number;
  public valor: string;

  constructor(id?: number, descripcion?: string) {
    this.clave = id;
    this.valor = descripcion;
  }
}
