export class GarantePrestamo {
  public apellido: string;
  public nombre: string;
  public nroDocumento: string;
  public nroFormulario: number;
  public localidad: string;
  public departamento: string;
  public fechaNacimiento: Date;

  constructor() {
    this.apellido = undefined;
    this.nombre = undefined;
    this.nroDocumento = undefined;
    this.nroFormulario = undefined;
    this.localidad = undefined;
    this.departamento = undefined;
    this.fechaNacimiento = undefined;

  }
}
