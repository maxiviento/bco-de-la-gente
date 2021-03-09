export class DesagruparLoteComando {
  public idLote: number;
  public idPrestamosDesagrupados: number[];

  constructor(idLote?: number,
              idPrestamosDesagrupados?: number[]) {
    this.idLote = idLote;
    this.idPrestamosDesagrupados = idPrestamosDesagrupados;
  }
}
