export class FechasPago {
  public modPago: string;
  public elementoPago: string;
  public convenioPago: number;
  public fecInicioPago: Date;
  public fecFinPago: Date;

  constructor(modPago?: string,
              elementoPago?: string,
              convenioPago?: number,
              fecInicioPago?: Date,
              fecFinPago?: Date) {
    this.modPago = modPago;
    this.elementoPago = elementoPago;
    this.convenioPago = convenioPago;
    this.fecFinPago = fecFinPago;
    this.fecInicioPago = fecInicioPago;
  }
}
