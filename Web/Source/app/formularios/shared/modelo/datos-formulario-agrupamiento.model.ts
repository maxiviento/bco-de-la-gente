export class DatosFormularioAgrupamiento {
  public idFormulario: number;
  public nroDocumento: string;
  public nombreCompleto: string;
  public codigoArea: string;
  public telefono: string;
  public codigoAreaCelular: string;
  public celular: string;
  public email: string;
  public codigoPais: string;

  constructor(
    idFormulario?: number,
    nroDocumento?: string,
    nombreCompleto?: string,
    codigoArea?: string,
    telefono?: string,
    codigoAreaCelular?: string,
    celular?: string,
    email?: string,
    codigoPais?: string) {
    this.idFormulario = idFormulario;
    this.nroDocumento = nroDocumento;
    this.nombreCompleto = nombreCompleto;
    this.codigoArea = codigoArea;
    this.telefono = telefono;
    this.codigoAreaCelular = codigoAreaCelular;
    this.celular = celular;
    this.email = email;
    this.codigoPais = codigoPais;
  }
}
