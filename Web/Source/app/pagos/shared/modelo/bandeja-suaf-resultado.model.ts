export class BandejaSuafResultado {
  public idLote: number;
  public nombreLote: string;
  public cantBeneficiarios: number;
  public idTipoLote: number;
  constructor(idLote?: number,
              nombreLote?: string,
              cantBeneficiarios?: number,
              idTipoLote?: number) {
    this.idLote = idLote;
    this.nombreLote = nombreLote;
    this.cantBeneficiarios = cantBeneficiarios;
    this.idTipoLote = idTipoLote;
  }
}
