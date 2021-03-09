export class MotivorechazoPrestamo {
  public idMotivo: number;
  public fechaRechazo: Date;
  public nombreMotivo: string;
  public observaciones: string;
  public idSeguimientoPrestamo: number;
  public descripcion: string;
  public abreviatura: string;

  constructor(
    idMotivo?: number,
    fechaRechazo?: Date,
    nombreMotivo?: string,
    observaciones?: string,
    idSeguimientoPrestamo?: number,
    descripcion?: string,
    abreviatura?: string
  ) {
    this.idMotivo = idMotivo;
    this.fechaRechazo = fechaRechazo;
    this.nombreMotivo = nombreMotivo;
    this.observaciones = observaciones;
    this.idSeguimientoPrestamo = idSeguimientoPrestamo;
    this.descripcion = descripcion;
    this.abreviatura = abreviatura;
  }
}
