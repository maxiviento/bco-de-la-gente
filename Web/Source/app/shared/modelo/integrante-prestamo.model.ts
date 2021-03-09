export class IntegrantePrestamo {
  public apellidoNombre: string;
  public cuil: string;
  public sexoId: string;
  public codigoPais: string;
  public nroDocumento: string;
  public estadoFormulario: string;
  public nroSticker: string;
  public nroAgrupamiento: number;
  public nroFormulario: number;
  public localidad: string;
  public departamento: string;
  public origenFormulario: string;
  public nroLinea: number;
  public idFormulario: number;
  public fechaNacimiento: Date;
  public nombreBanco: string;
  public nombreSucursal: string;
  public motivoRechazo: string;
  public numeroCaja: string;
  public tipoIntegrante: number;
  public numDevengado: string;
  public tieneDeuda: boolean;

  constructor() {
    this.apellidoNombre = undefined;
    this.cuil = undefined;
    this.estadoFormulario = undefined;
    this.nroSticker = undefined;
    this.nroAgrupamiento = undefined;
    this.nroFormulario = undefined;
    this.localidad = undefined;
    this.departamento = undefined;
    this.origenFormulario = undefined;
    this.nroLinea = undefined;
    this.idFormulario = undefined;
    this.fechaNacimiento = undefined;
    this.nombreBanco = undefined;
    this.nombreSucursal = undefined;
    this.motivoRechazo = undefined;
    this.numeroCaja = undefined;
    this.tipoIntegrante = undefined;
    this.sexoId = undefined;
    this.codigoPais = undefined;
    this.nroDocumento = undefined;
    this.numDevengado = undefined;
    this.tieneDeuda = undefined;
  }
}
