export class InconsistenciaArchivoConsulta {
  public idCabecera: number;
  public numeroPagina: number;
  public tamañoPagina: number;

  constructor(idCabecera?: number,
              numeroPagina?: number,
              tamañoPagina?: number) {
    this.idCabecera = idCabecera;
    this.numeroPagina = numeroPagina;
    this.tamañoPagina = tamañoPagina;
  }
}
