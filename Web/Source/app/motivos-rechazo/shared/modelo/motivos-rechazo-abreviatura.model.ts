export class MotivoRechazoAbreviatura {
  public abreviatura: string;
  public dadoDeBaja: boolean;
  public idAmbito: number;
  public automatico: boolean;

  constructor(abreviatura?: string,
              dadoDeBaja?: boolean,
              idAmbito?: number,
              automatico?: boolean) {
    this.abreviatura = abreviatura;
    this.dadoDeBaja = dadoDeBaja;
    this.idAmbito = idAmbito;
    this.automatico = automatico;
  }
}
