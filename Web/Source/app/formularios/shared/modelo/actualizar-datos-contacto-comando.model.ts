export class ActualizarDatosDeContactoComando {
  public idSexo: string;
  public codigoPais: string;
  public nroDocumento: string;
  public idNumero: string;

  public codAreaTelefono: string;
  public nroTelefono: string;
  public codAreaCelular: string;
  public nroCelular: string;
  public mail: string;

  constructor(idSexo?: string,
              codigoPais?: string,
              nroDocumento?: string,
              idNumero?: string,
              codAreaTelefono?: string,
              nroTelefono?: string,
              codAreaCelular?: string,
              nroCelular?: string,
              mail?: string) {
    this.idSexo = idSexo;
    this.codigoPais = codigoPais;
    this.nroDocumento = nroDocumento;
    this.idNumero = idNumero;

    this.codAreaTelefono = codAreaTelefono;
    this.nroTelefono = nroTelefono;
    this.codAreaCelular = codAreaCelular;
    this.nroCelular = nroCelular;
    this.mail = mail;
  }
}
