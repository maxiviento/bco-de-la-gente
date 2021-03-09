export class FormulariosSeleccionadosAdendaConsulta {
  public nroDetalle: number;
  public numeroPagina: number;
  public tamañoPagina: number;

  constructor(nroDetalle?: number,
              numeroPagina?: number,
              tamañoPagina?: number) {
    this.nroDetalle = nroDetalle;
    this.numeroPagina = numeroPagina;
    this.tamañoPagina = tamañoPagina;
  }
}
