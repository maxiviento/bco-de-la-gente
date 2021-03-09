export class CambiarGaranteComando {
  public sexoId: string;
  public codigoPais: string;
  public nroDocumento: string;
  public idGaranteFormulario: number;
  public idNumero: number;

  constructor(sexoId?: string,
              codigoPais?: string,
              nroDocumento?: string,
              idGaranteFormulario?: number,
              idNumero?: number) {
    this.sexoId = sexoId;
    this.codigoPais = codigoPais;
    this.nroDocumento = nroDocumento;
    this.idGaranteFormulario = idGaranteFormulario;
    this.idNumero = idNumero;
  }
}
