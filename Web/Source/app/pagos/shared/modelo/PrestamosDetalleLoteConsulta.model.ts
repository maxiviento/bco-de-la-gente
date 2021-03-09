export class PrestamosDetalleLoteConsulta {
  public idLote: number;
  public esVer: boolean;
  public numeroPagina: number;
  public tamañoPagina: number;

  constructor(idLote?: number,
              esVer?: boolean,
              numeroPagina?: number,
              tamañoPagina?: number) {
    this.idLote = idLote;
    this.esVer = esVer;
    this.numeroPagina = numeroPagina;
    this.tamañoPagina = tamañoPagina;
  }
}
