export class EdicionEtapaComando {
  public nombre: string;
  public descripcion: string;

  constructor(nombre?: string,
              descripcion?: string) {
    this.nombre = nombre;
    this.descripcion = descripcion;
  }
}
