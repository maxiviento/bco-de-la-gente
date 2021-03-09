export class ImportarArchivoResultadoBancoResultado {
  public resultado: string;
  public coincidenMontos: boolean;
  public hayError: boolean;

  constructor(resultado?: string,
              coincidenMontos?: boolean,
              hayError?: boolean) {
    this.resultado = resultado;
    this.coincidenMontos = coincidenMontos;
    this.hayError = hayError;
  }
}
