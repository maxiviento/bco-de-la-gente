export class DatosPersona {
  public idSexo: number;
  public nroDocumento: string;
  public codigoPais: string;
  public numero: string;

  constructor(idSexo?: number,
              nroDocumento?: string,
              codigoPais?: string,
              numero?: string) {
    this.idSexo = idSexo;
    this.nroDocumento = nroDocumento;
    this.codigoPais = codigoPais;
    this.numero = numero;
  }
}
