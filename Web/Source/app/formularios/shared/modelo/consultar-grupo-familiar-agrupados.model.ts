export class ConsultarGrupoFamiliarAgrupados {
  public idSexo: string;
  public nroDocumento: string;
  public codigoPais: string;
  public idNumero: string;
  public idAgrupamiento: number;

  constructor(idSexo?: string,
              nroDocumento?: string,
              codigoPais?: string,
              idNumero?: string,
              idAgrupamiento?: number) {
    this.idSexo = idSexo;
    this.nroDocumento = nroDocumento;
    this.codigoPais = codigoPais;
    this.idNumero = idNumero;
    this.idAgrupamiento = idAgrupamiento;
  }
}
