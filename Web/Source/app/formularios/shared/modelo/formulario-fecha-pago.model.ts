export class FormularioFechaPago {
  public idFormulario: number;
  public fecInicioPago: Date;
  public fecFinPago: Date;
  public esAsociativa: boolean;
  public cantMinForms: number;
  public estadoForm: number;
  public cantForms: number;
  public tipoApoderado: number;

  constructor(idFormulario?: number,
              fecInicioPago?: Date,
              fecFinPago?: Date,
              esAsociativa?: boolean,
              cantMinForms?: number,
              estadoForm?: number,
              cantForms?: number,
              tipoApoderado?: number) {
    this.idFormulario = idFormulario;
    this.fecInicioPago = fecInicioPago;
    this.fecFinPago = fecFinPago;
    this.esAsociativa = esAsociativa;
    this.cantMinForms = cantMinForms;
    this.estadoForm = estadoForm;
    this.cantForms = cantForms;
    this.tipoApoderado = tipoApoderado;
  }
}
