export class BandejaConformarPrestamoConsulta {
  public cuilSolicitante: string;
  public fechaDesde: Date;
  public fechaHasta: Date;
  public departamentoIds: string;
  public localidadIds: string;
  public numeroFormulario: number;
  public idOrigen: number;
  public idEstadoFormulario: string;
  public idLinea: string;
  public numeroPrestamo: number;
  public numeroSticker: string;
  public tipoPersona: number;
  public cuil: string;
  public apellido: string;
  public nombre: string;
  public dni: string;
  public numeroPagina: number;
  public tamañoPagina: number;
  public tipoApoderado: number;

  constructor(cuilSolicitante?: string,
              fechaDesde?: Date,
              fechaHasta?: Date,
              dni?: string,
              idOrigen?: number,
              idEstadoFormulario?: string,
              NumeroFormulario?: number,
              idLinea?: string,
              numeroPrestamo?: number,
              numeroSticker?: string,
              tipoPersona?: number,
              cuil?: string,
              apellido?: string,
              nombre?: string,
              numeroPagina?: number,
              tamañoPagina?: number,
              tipoApoderado?: number,
              orderByDes?: boolean,
              columnaOrderBy?: number,
              departamentoIds?: string,
              localidadIds?: string) {

    this.cuilSolicitante = cuilSolicitante;
    this.fechaDesde = fechaDesde;
    this.fechaHasta = fechaHasta;
    this.dni = dni;
    this.idOrigen = idOrigen;
    this.idEstadoFormulario = idEstadoFormulario;
    this.numeroFormulario = NumeroFormulario;
    this.idLinea = idLinea;
    this.numeroPrestamo = numeroPrestamo;
    this.numeroSticker = numeroSticker;
    this.tipoPersona = tipoPersona;
    this.cuil = cuil;
    this.apellido = apellido;
    this.nombre = nombre;
    this.numeroPagina = numeroPagina;
    this.tamañoPagina = tamañoPagina;
    this.tipoApoderado = tipoApoderado;
    this.localidadIds = localidadIds;
    this.departamentoIds = departamentoIds;
  }
}
