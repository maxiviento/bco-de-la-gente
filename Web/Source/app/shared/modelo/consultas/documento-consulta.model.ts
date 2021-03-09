export class DocumentoConsulta {
  public idItem: number;
  public idFormularioLinea: number;
  public numeroPagina: number;
  public tamañoPagina: number;

  constructor(idItem?: number,
              idFormularioLinea?: number,
              numeroPagina?: number,
              tamañoPagina?: number) {

    this.idItem = idItem;
    this.idFormularioLinea = idFormularioLinea;
    this.numeroPagina = numeroPagina;
    this.tamañoPagina = tamañoPagina;
  }
}
