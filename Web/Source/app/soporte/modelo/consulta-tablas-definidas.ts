export class ConsultaTablasDefinidas {
  public idTabla: number;
  public nombre: string;
  public numeroPagina: number;
  public tamañoPagina: number;

  constructor(idTabla?: number,
              filtroNombre?: string,
              numeroPagina?: number,
              tamañoPagina?: number) {
    this.idTabla = idTabla;
    this.nombre = filtroNombre;
    this.numeroPagina = numeroPagina;
    this.tamañoPagina = tamañoPagina;
  }
}
