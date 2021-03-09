export class ConsultaMotivoDestino {
  public nombre: string;
  public incluirDadosDeBaja: boolean;
  public numeroPagina: number;
  public tamañoPagina: number;

  constructor() {
    this.nombre = undefined;
    this.incluirDadosDeBaja = undefined;
    this.numeroPagina = undefined;
    this.tamañoPagina = undefined;
  }
}
