export class ArchivoRecupero {
  public Archivo: any;
  public idTipoEntidad: number;
  public fechaRecupero: Date;
  public convenio: number;

  constructor(archivo?: any,
              idTipoEntidad?: number,
              fechaRecupero?: Date,
              convenio?: number) {
    this.Archivo = archivo;
    this.idTipoEntidad = idTipoEntidad;
    this.fechaRecupero = fechaRecupero;
    this.convenio = convenio;
  }
}
