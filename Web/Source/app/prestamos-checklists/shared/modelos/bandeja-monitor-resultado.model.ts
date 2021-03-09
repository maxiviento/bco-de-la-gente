export class BandejaMonitorResultado {
  public id: number;
  public fechaAlta: Date;
  public fechaInicioProceso: Date;
  public fechaGeneracionPdf: Date;
  public fechaModificacion: Date;
  public idEstado: number;
  public estado: string;
  public idTipo: number;
  public tipo: string;
  public usuarioModificacion: string;
  public usuarioAlta: string;
  public pathArchivo: string;

  constructor(
    id?: number,
    fechaAlta?: Date,
    fechaInicioProceso?: Date,
    fechaGeneracionPdf?: Date,
    fechaModificacion?: Date,
    idEstado?: number,
    estado?: string,
    idTipo?: number,
    tipo?: string,
    usuarioModificacion?: string,
    usuarioAlta?: string,
    pathArchivo?: string
  ) {
    this.id = id;
    this.fechaAlta = fechaAlta;
    this.fechaInicioProceso = fechaInicioProceso;
    this.fechaGeneracionPdf = fechaGeneracionPdf;
    this.fechaModificacion = fechaModificacion;
    this.idEstado = idEstado;
    this.estado = estado;
    this.idTipo = idTipo;
    this.tipo = tipo;
    this.usuarioModificacion = usuarioModificacion;
    this.usuarioAlta = usuarioAlta;
    this.pathArchivo = pathArchivo;
  }
}
