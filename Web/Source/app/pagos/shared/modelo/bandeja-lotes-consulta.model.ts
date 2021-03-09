export class BandejaLotesConsulta {
  public fechaInicio: Date;
  public fechaFin: Date;
  public nroLoteDesde: number;
  public nroLoteHasta: number;
  public nroPrestamo: number;
  public idLinea: number;
  public numeroPagina: number;
  public tamañoPagina: number;

  public tipoPersona: number;
  public cuil: string;
  public apellido: string;
  public nombre: string;
  public dni: string;

  constructor(fechaInicio?: Date,
              fechaFin?: Date,
              nroLoteDesde?: number,
              nroLoteHasta?: number,
              convenios?: number[],
              nroPrestamo?: number,
              idLinea?: number,
              tipoPersona?: number,
              cuil?: string,
              apellido?: string,
              nombre?: string,
              dni?: string) {
    this.fechaInicio = fechaInicio;
    this.fechaFin = fechaFin;
    this.nroLoteDesde = nroLoteDesde;
    this.nroLoteHasta = nroLoteHasta;
    this.nroPrestamo = nroPrestamo;
    this.idLinea = idLinea;
    this.tipoPersona = tipoPersona;
    this.cuil = cuil;
    this.apellido = apellido;
    this.nombre = nombre;
    this.dni = dni;
  }
}
