export class BandejaCambioEstadoConsulta {
  public fechaDesde: Date;
  public fechaHasta: Date;
  public nroFormulario: number;
  public nroPrestamo: number;
  public idLinea: number;
  public idEstadoFormulario: number;
  public idElementoPago: number;
  public numeroPagina: number;
  public tamañoPagina: number;
  public cuil: string;
  public dni: string;
  public idDepartamento: number;
  public idLocalidad: number;
  public nroSticker: string;

  constructor(fechaDesde?: Date,
              fechaHasta?: Date,
              nroFormulario?: number,
              nroPrestamo?: number,
              idLinea?: number,
              idEstadoFormulario?: number,
              idElementoPago?: number,
              numeroPagina?: number,
              tamañoPagina?: number,
              cuil?: string,
              dni?: string,
              idDepartamento?: number,
              idLocalidad?: number,
              nroSticker?: string) {
    this.fechaDesde = fechaDesde;
    this.fechaHasta = fechaHasta;
    this.nroFormulario = nroFormulario;
    this.nroPrestamo = nroPrestamo;
    this.idLinea = idLinea;
    this.idEstadoFormulario = idEstadoFormulario;
    this.idElementoPago = idElementoPago;
    this.numeroPagina = numeroPagina;
    this.tamañoPagina = tamañoPagina;
    this.cuil = cuil;
    this.dni = dni;
    this.idDepartamento = idDepartamento;
    this.idLocalidad = idLocalidad;
    this.nroSticker = nroSticker;
  }
}
