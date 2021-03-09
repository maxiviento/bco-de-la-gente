export class BandejaMontoDisponibleResultado {
  public id: number;
  public fechaDepositoBancario: Date;
  public fechaInicio: Date;
  public fechaFin: Date;
  public nroMonto: number;
  public descripcion: string;
  public idMotivoBaja: number;
  public saldo: number;
  public fechaUltimaModificacion: Date;

  constructor(fechaInicio?: Date,
              fechaFin?: Date,
              fechaDepositoBancario?: Date,
              nroMonto?: number,
              id?: number,
              descripcion?: string,
              idMotivoBaja?: number,
              saldo?: number,
              fechaUltimaModificacion?: Date) {
    this.fechaInicio = fechaInicio;
    this.fechaFin = fechaFin;
    this.fechaDepositoBancario = fechaDepositoBancario;
    this.nroMonto = nroMonto;
    this.id = id;
    this.descripcion = descripcion;
    this.idMotivoBaja = idMotivoBaja;
    this.saldo = saldo;
    this.fechaUltimaModificacion = fechaUltimaModificacion;
  }
}
