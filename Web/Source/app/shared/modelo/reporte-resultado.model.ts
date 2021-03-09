import {Archivo} from "./archivo.model";

export class ReporteResultado{
  public archivos: Archivo[];
  public errores: string[];


  constructor(archivos?: Archivo[], errores?: string[]) {
    this.archivos = archivos;
    this.errores = errores;
  }
}
