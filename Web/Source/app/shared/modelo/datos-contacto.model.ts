export class DatosContacto {
  public codigoArea: string;
  public telefono: string;
  public codigoAreaCelular: string;
  public celular: string;
  public mail: string;

  constructor(codigoArea?: string,
              telefono?: string,
              codigoAreaCelular?: string,
              celular?: string,
              mail?: string) {
    this.codigoArea = codigoArea;
    this.telefono = telefono;
    this.codigoAreaCelular = codigoAreaCelular;
    this.celular = celular;
    this.mail = mail;
  }
}
