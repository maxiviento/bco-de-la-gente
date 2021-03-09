export class BandejaResultadoBancoResultado {
  public idCabecera: number;
  public fechaRecepcion: Date;
  public importe: number;
  public periodo: string;
  public formaPago: string;
  public tipoPago: string;
  public banco: string;

  constructor(idCabecera?: number,
              fechaRecepcion?: Date,
              importe?: number,
              periodo?: string,
              formaPago?: string,
              tipoPago?: string,
              banco?: string) {
    this.idCabecera = idCabecera;
    this.fechaRecepcion = fechaRecepcion;
    this.importe = importe;
    this.periodo = periodo;
    this.formaPago = formaPago;
    this.tipoPago = tipoPago;
    this.banco = banco;
  }
}
