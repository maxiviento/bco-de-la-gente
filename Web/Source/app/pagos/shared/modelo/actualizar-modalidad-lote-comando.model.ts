export class ActualizarModalidadLoteComando {
  public idLote: number;
  public fechaPago: Date;
  public fechaFinPago: Date;
  public elementoPago: number;
  public modalidadPago: number;
  public convenioPago: number;
  public generaPlanCuotas: boolean;
  public mesesGracia: number;

  constructor(idLote?: number,
              fechaPago?: Date,
              fechaFinPago?: Date,
              elementoPago?: number,
              convenioPago?: number,
              generaPlanCuotas?: boolean,
              mesesGracia?: number,
              modalidadPago?: number) {
    this.idLote = idLote;
    this.fechaPago = fechaPago;
    this.fechaFinPago = fechaFinPago;
    this.elementoPago = elementoPago;
    this.modalidadPago = modalidadPago;
    this.convenioPago = convenioPago;
    this.generaPlanCuotas = generaPlanCuotas;
    this.mesesGracia = mesesGracia;
  }
}
