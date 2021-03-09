export class BandejaChequeResultado {
  public idFormulario: number;
  public idPrestamo: number;
  public linea: string;
  public departamento: string;
  public localidad: string;
  public apellidoNombreSolicitante: string;
  public nroPrestamo: number;
  public nroFormulario: number;
  public nroCheque: string;
  public nroChequeNuevo: string;
  public fechaVencimientoCheque: Date;
  public fechaChequeNuevo: Date;
  public origen: string;
  public cuilSolicitante: string;

  constructor(idFormulario?: number,
              idPrestamo?: number,
              linea?: string,
              departamento?: string,
              localidad?: string,
              apellidoNombreSolicitante?: string,
              nroPrestamo?: number,
              nroFormulario?: number,
              nroCheque?: string,
              nroChequeNuevo?: string,
              fechaVencimientoCheque?: Date,
              fechaChequeNuevo?: Date,
              origen?: string,
              cuilSolicitante?: string) {
    this.idFormulario = idFormulario;
    this.idPrestamo = idPrestamo;
    this.linea = linea;
    this.departamento = departamento;
    this.localidad = localidad;
    this.nroPrestamo = nroPrestamo;
    this.nroFormulario = nroFormulario;
    this.nroCheque = nroCheque;
    this.nroChequeNuevo = nroChequeNuevo;
    this.fechaVencimientoCheque = fechaVencimientoCheque;
    this.apellidoNombreSolicitante = apellidoNombreSolicitante;
    this.fechaChequeNuevo = fechaChequeNuevo;
    this.origen = origen;
    this.cuilSolicitante = cuilSolicitante;
  }
}
