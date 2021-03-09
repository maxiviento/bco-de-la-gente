export class DatosPrestamoReactivacionResultado {
  public apellidoSolicitante: string;
  public nombreSolicitante: string;
  public idFormulario: number;
  public nroFormulario: string;
  public idPrestamo: number;
  public nroPrestamo: string;
  public nroCaja: string;
  public idSexo: string;
  public numeroDocumento: string;
  public codigoPais: string;
  public idNumero: string;
  public idSexoGarante: string;
  public numeroDocumentoGarante: string;
  public codigoPaisGarante: string;
  public idNumeroGarante: string;


  constructor(apellidoSolicitante?: string,
              nombreSolicitante?: string,
              idFormulario?: number,
              nroFormulario?: string,
              idPrestamo?: number,
              nroPrestamo?: string,
              nroCaja?: string,
              sexoIdSolicitante?: string,
              nroDocumentoSolicitante?: string,
              codigoPaisSolicitante?: string,
              idNumeroSolicitante?: string,
              sexoIdGarante?: string,
              nroDocumentoGarante?: string,
              codigoPaisGarante?: string,
              idNumeroGarante?: string
  ) {
    this.idFormulario = idFormulario;
    this.idPrestamo = idPrestamo;
    this.nroFormulario = nroFormulario;
    this.nroPrestamo = nroPrestamo;
    this.nombreSolicitante = nombreSolicitante;
    this.apellidoSolicitante = apellidoSolicitante;
    this.nroCaja = nroCaja;
    this.idSexo = sexoIdSolicitante;
    this.numeroDocumento = nroDocumentoSolicitante;
    this.codigoPais = codigoPaisSolicitante;
    this.idNumero = idNumeroSolicitante;
    this.idSexoGarante = sexoIdGarante;
    this.numeroDocumentoGarante = nroDocumentoGarante;
    this.codigoPaisGarante = codigoPaisGarante;
    this.idNumeroGarante = idNumeroGarante;
  }
}
