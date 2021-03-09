export class ConsultaParametrosTablasDefinidas {
  public idTabla: number;
  public nombre: string;
  public idParametro: number;
  public incluirDadosDeBaja: boolean = false;
  public numeroPagina: number;
  public tamañoPagina: number;

  constructor(idTabla?: number,
              filtroNombre?: string,
              idParametro?: number,
              incluirDadosDeBaja?: boolean,
              numeroPagina?: number,
              tamañoPagina?: number) {
    this.idTabla = idTabla;
    this.nombre = filtroNombre;
    this.idParametro = this.idParametro;
    this.incluirDadosDeBaja = incluirDadosDeBaja;
    this.numeroPagina = numeroPagina;
    this.tamañoPagina = tamañoPagina;
  }
}
