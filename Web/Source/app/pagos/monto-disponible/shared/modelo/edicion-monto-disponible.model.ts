export class EdicionMontoDisponibleComando {
  public descripcion: string;
  public idBanco: number;
  public idSucursal: number;
  public monto: number;
  public fechaDepositoBancario: Date;
  public fechaInicioPago: Date;
  public fechaFinPago: Date;

  constructor(descripcion?: string,
              idBanco?: number,
              idSucursal?: number,
              monto?: number,
              fechaDepositoBancario?: Date,
              fechaInicioPago?: Date,
              fechaFinPago?: Date) {
    this.descripcion = descripcion;
    this.idBanco = idBanco;
    this.idSucursal = idSucursal;
    this.monto = monto;
    this.fechaDepositoBancario = fechaDepositoBancario;
    this.fechaInicioPago = fechaInicioPago;
    this.fechaFinPago = fechaFinPago;
  }
}
