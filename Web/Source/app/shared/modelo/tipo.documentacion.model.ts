export class TipoDocumentacion {
  private id: number;
  private descripcion: string;
  private nombre: string;


  constructor(id?: number, descripcion?: string, nombre?: string) {
    this.id = id;
    this.descripcion = descripcion;
    this.nombre = nombre;
  }
}
