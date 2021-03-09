export class BandejaChequeConsulta {
  public fechaDesde: Date;
  public fechaHasta: Date;
  public idLocalidad: number;
  public idDepartamento: number;
  public idOrigen: number;
  public nombre: string;
  public apellido: string;
  public numeroFormulario: number;
  public idLote: number;
  public idLinea: number;
  public tipoPersona: number;
  public cuil: string;
  public dni: string;
  public numeroPagina: number;
  public tamañoPagina: number;
  public numeroPrestamo: number;

  constructor(fechaDesde?: Date,
              fechaHasta?: Date,
              idDepartamento?: number,
              idLocalidad?: number,
              idOrigen?: number,
              nombre?: string,
              apellido?: string,
              numeroFormulario?: number,
              idLote?: number,
              idLinea?: number,
              tipoPersona?: number,
              cuil?: string,
              dni?: string,
              numeroPagina?: number,
              tamañoPagina?: number,
              numeroPrestamo?: number,
           ) {

    this.fechaDesde = fechaDesde;
    this.fechaHasta = fechaHasta;
    this.idLocalidad = idLocalidad;
    this.idDepartamento = idDepartamento;
    this.idOrigen = idOrigen;
    this.nombre = nombre;
    this.apellido = apellido;
    this.numeroFormulario = numeroFormulario;
    this.dni = dni;
    this.idLote = idLote;
    this.idLinea = idLinea;
    this.numeroPagina = numeroPagina;
    this.tamañoPagina = tamañoPagina;
    this.numeroPrestamo = numeroPrestamo;
  }
}
