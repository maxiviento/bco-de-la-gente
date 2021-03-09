export class MovimientoMontoConsulta {
  public idMonto: number;
  public numeroPagina: number;

  constructor(idMonto?: number,
              numeroPagina?: number) {
    this.idMonto = idMonto;
    this.numeroPagina = numeroPagina;
  }
}
