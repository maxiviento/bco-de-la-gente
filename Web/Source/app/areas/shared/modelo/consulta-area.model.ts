export class ConsultaArea {
  public nombre: string;
  public incluirDadosDeBaja: boolean = false;
  public numeroPagina: number;
  public tamañoPagina: number;

  constructor(nombre?: string,
              incluirDadosDeBaja?: boolean,
              numeroPagina?: number,
              tamañoPagina?: number) {
    this.nombre = nombre;
    this.incluirDadosDeBaja = incluirDadosDeBaja;
    this.numeroPagina = numeroPagina;
    this.tamañoPagina = tamañoPagina;
  }
}
