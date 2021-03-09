export class ConsultaParametro {
  public id: number;
  public incluirNoVigentes: boolean = false;
  public numeroPagina: number;
  public tamanioPagina: number;

  constructor(id?: number,
              incluirNoVigentes?: boolean,
              numeroPagina?: number,
              tamanioPagina?: number) {
    this.id = id;
    this.incluirNoVigentes = incluirNoVigentes;
    this.numeroPagina = numeroPagina;
    this.tamanioPagina = tamanioPagina;
  }
}
