export class ConsultaConfiguracionChecklist {
  public idLinea: number;
  public idEtapa: number;

  constructor(lineaId?: number, etapaId?: number) {
    this.idLinea = lineaId;
    this.idEtapa = etapaId;
  }
}
