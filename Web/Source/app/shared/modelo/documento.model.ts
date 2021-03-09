export class Documento {
  public id: number;
  public idDocumentoCidi: number;
  public nombreArchivo: string;
  public fechaAlta: Date;
  public extension: string;
  public nombreUsuario: string;

  constructor(id?: number, idDocumentoCidi?: number, nombre?: string, fecha?: Date, extension?: string, nombreUsuario?: string) {
    this.idDocumentoCidi = idDocumentoCidi;
    this.id = id;
    this.nombreArchivo = nombre;
    this.fechaAlta = fecha;
    this.extension = extension;
    this.nombreUsuario = nombreUsuario;
  }
}
