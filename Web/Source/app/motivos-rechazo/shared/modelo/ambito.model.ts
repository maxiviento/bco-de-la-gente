export class Ambito {
  public nombre: string;
  public descripcion: string;
  public id: number;


  constructor(nombre?: string, descripcion?: string, id?: number) {
    this.nombre = nombre;
    this.descripcion = descripcion;
    this.id = id;
  }
}
