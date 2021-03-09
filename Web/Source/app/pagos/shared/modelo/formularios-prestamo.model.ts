export class FormularioPrestamo {
  public beneficiario: string;
  public cuil: string;
  public origen: string;
  public nroFormulario: string;
  public montoFormulario: number;
  public idFormulario: number;
  public fechaPago: Date;
  public estado: string;
  public apellidoNombre: string;

  constructor(beneficiario?: string,
              cuil?: string,
              origen?: string,
              nroFormulario?: string,
              montoFormulario?: number,
              idFormulario?: number,
              fechaPago?: Date,
              estado?: string,
              apellidoNombre?: string) {
    this.beneficiario = beneficiario;
    this.cuil = cuil;
    this.origen = origen;
    this.nroFormulario = nroFormulario;
    this.montoFormulario = montoFormulario;
    this.idFormulario = idFormulario;
    this.fechaPago = fechaPago;
    this.estado = estado;
    this.apellidoNombre = apellidoNombre;
  }
}
