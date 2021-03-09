export class TipoPagoCombo {
  public idTipoPago: number;
  public tipoPago: string;
  public codigo: string;

  constructor(idTipoPago?: number,
              tipoPago?: string,
              codigo?: string) {
    this.idTipoPago = idTipoPago;
    this.tipoPago = tipoPago;
    this.codigo = codigo;
  }
}
