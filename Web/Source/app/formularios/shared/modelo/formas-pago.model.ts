export class FormasPago {
  public porcentajeContadoEfectivoCompra: string;
  public porcentajeCreditoProveedoresCompra: string;
  public creditoProveedoresPlazoPagoCompra: string;
  public porcentajeOtraFormaPagoCompra: string;
  public otraFormaPagoPlazoCompra: string;
  public porcentajeContadoEfectivoVenta: string;
  public porcentajeCreditoProveedoresVenta: string;
  public creditoProveedoresPlazoPagoVenta: string;
  public porcentajeOtraFormaPagoVenta: string;
  public otraFormaPagoPlazoVenta: string;

  constructor(porcentajeContadoEfectivoCompra?: string,
              porcentajeCreditoProveedoresCompra?: string,
              creditoProveedoresPlazoPagoCompra?: string,
              porcentajeOtraFormaPagoCompra?: string,
              otraFormaPagoPlazoCompra?: string,
              porcentajeContadoEfectivoVenta?: string,
              porcentajeCreditoProveedoresVenta?: string,
              creditoProveedoresPlazoPagoVenta?: string,
              porcentajeOtraFormaPagoVenta?: string,
              otraFormaPagoPlazoVenta?: string) {
    this.porcentajeContadoEfectivoCompra = porcentajeContadoEfectivoCompra;
    this.porcentajeCreditoProveedoresCompra = porcentajeCreditoProveedoresCompra;
    this.creditoProveedoresPlazoPagoCompra = creditoProveedoresPlazoPagoCompra;
    this.porcentajeOtraFormaPagoCompra = porcentajeOtraFormaPagoCompra;
    this.otraFormaPagoPlazoCompra = otraFormaPagoPlazoCompra;
    this.porcentajeContadoEfectivoVenta = porcentajeContadoEfectivoVenta;
    this.porcentajeCreditoProveedoresVenta = porcentajeCreditoProveedoresVenta;
    this.creditoProveedoresPlazoPagoVenta = creditoProveedoresPlazoPagoVenta;
    this.porcentajeOtraFormaPagoVenta = porcentajeOtraFormaPagoVenta;
    this.otraFormaPagoPlazoVenta = otraFormaPagoPlazoVenta;
  }
}
