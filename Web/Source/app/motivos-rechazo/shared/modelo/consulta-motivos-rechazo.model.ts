export class ConsultaMotivosRechazo {
  public abreviatura: string;
  public ambitoId: number;
  public automatico: boolean;
  public codigo: string;
  public incluirDadosDeBaja: boolean;
  public numeroPagina: number;
  public tamañoPagina: number;

  constructor() {
    this.abreviatura = undefined;
    this.incluirDadosDeBaja = undefined;
    this.codigo = undefined;
    this.numeroPagina = undefined;
    this.tamañoPagina = undefined;
    this.ambitoId = undefined;
    this.automatico = null;
  }
}
