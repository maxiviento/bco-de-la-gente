export class ObtenerDatosContactoConsulta {
  public idSexo: string;
  public nroDocumento: string;
  public codigoPais: string;
  public idNumero: string;

  constructor(idSexo?: string,
              nroDocumento?: string,
              codigoPais?: string,
              idNumero?: string) {
    this.idSexo = idSexo;
    this.nroDocumento = nroDocumento;
    this.codigoPais = codigoPais;
    this.idNumero = idNumero;
  }
}
