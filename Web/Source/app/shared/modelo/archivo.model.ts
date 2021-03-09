import {TipoArchivo} from "./tipo-archivo.model";

export class Archivo{
  public archivo: string;
  public tipo: TipoArchivo;
  public nombre: string;
  public nombreConExtension: string;


  constructor(archivo?: string,
              tipo?: TipoArchivo,
              nombre?: string,
              nombreConExtension?: string) {
    this.archivo = archivo;
    this.tipo = tipo;
    this.nombre = nombre;
    this.nombreConExtension = nombreConExtension;
  }
}
