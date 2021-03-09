export class BandejaPrestamoChecklistConsulta {
  public cuil: string;
  public fechaDesde: Date;
  public fechaHasta: Date;
  public idOrigen: number;
  public idEstadoFormulario: number;
  public idLinea: number;
  public apellido:string;
  public nombre:string;
  public numeroPagina: number;
  public tamañoPagina: number;

  constructor(cuil?: string,
              nombre?: string,
              apellido?: string,
              fechaDesde?: Date,
              fechaHasta?: Date,
              idOrigen?: number,
              idEstadoFormulario?: number,
              idLinea?: number,
              numeroPagina?: number,
              tamañoPagina?: number) {

    this.cuil = cuil;
    this.fechaDesde = fechaDesde;
    this.fechaHasta = fechaHasta;
    this.idOrigen = idOrigen;
    this.nombre = nombre;
    this.apellido = apellido;
    this.idEstadoFormulario = idEstadoFormulario;
    this.idLinea = idLinea;
    this.numeroPagina = numeroPagina;
    this.tamañoPagina = tamañoPagina;
  }
}
