export class ConsultaEtapa {
  public nombre: string;
  public incluirDadosDeBaja: boolean;
  public numeroPagina: number;
  public tamañoPagina: number;

  constructor(filtroNombre?: string,
              incluirDadosDeBaja?: boolean,
              numeroPagina?: number,
              tamañoPagina?: number) {
    this.nombre = filtroNombre;
    this.incluirDadosDeBaja = incluirDadosDeBaja;
    this.numeroPagina = numeroPagina;
    this.tamañoPagina = tamañoPagina;
  }
}
