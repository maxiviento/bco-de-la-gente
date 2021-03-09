export class ParametroTablaDefinida {
  public id: number;
  public nombre: string;
  public descripcion: string;
  public fechaDesde: Date;
  public fechaHasta: Date;
  public idMotivoRechazo: number;
  public nombreMotivoRechazo: string;

  constructor(id?: number, nombre?: string, descripcion?: string,  fechaDesde?: Date, fechaHasta?: Date, idMotivo?: number, nombreMotivoRechazo?: string) {
    this.id = id;
    this.nombre = nombre;
    this.descripcion = descripcion;
    this.fechaDesde = fechaDesde;
    this.fechaHasta = fechaHasta;
    this.idMotivoRechazo = idMotivo;
    this.nombreMotivoRechazo = nombreMotivoRechazo;
  }
}
