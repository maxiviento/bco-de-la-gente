export class ConsultaBandejaPrestamos {
  public nombre: string;
  public apellido: string;
  public cuil: string;
  public NroFormulario: string;
  public NroPrestamo: string;
  public NroSticker: string;
  public IdOrigen: number;
  public IdUsuario: number;
  public fechaDesde: Date;
  public fechaHasta: Date;
  public numeroPagina: number;
  public tamañoPagina: number;
  public tipoPersona: number;
  public dni: string;
  public quiereReactivar: boolean;
  public montoOtorgado: string;
  public orderByDes: boolean;
  public columnaOrderBy: number;
  public idEstadoPrestamo: string;
  public idLinea: string;
  public IdDepartamento: string;
  public IdLocalidad: string;
  public departamentoIds: string;
  public localidadIds: string;

  constructor(nombre?: string,
              apellido?: string,
              cuil?: string,
              NroFormulario?: string,
              NroPrestamo?: string,
              NroSticker?: string,
              idEstadoPrestamo?: string,
              IdOrigen?: number,
              idLinea?: string,
              IdUsuario?: number,
              departamentoIds?: string,
              localidadIds?: string,
              fechaDesde?: Date,
              fechaHasta?: Date,
              tipoPersona?: number,
              dni?: string,
              quiereReactivar?: boolean,
              montoOtorgado?: string,
              orderByDes?: boolean,
              columnaOrderBy?: number,
              idDepartamento?: string,
              idLocalidad?: string) {
    this.nombre = nombre;
    this.apellido = apellido;
    this.cuil = cuil;
    this.NroFormulario = NroFormulario;
    this.NroPrestamo = NroPrestamo;
    this.NroSticker = NroSticker;
    this.IdOrigen = IdOrigen;
    this.IdUsuario = IdUsuario;
    this.fechaDesde = fechaDesde;
    this.fechaHasta = fechaHasta;
    this.tipoPersona = tipoPersona;
    this.dni = dni;
    this.quiereReactivar = quiereReactivar;
    this.montoOtorgado = montoOtorgado;
    this.orderByDes = orderByDes;
    this.columnaOrderBy = columnaOrderBy;
    this.IdDepartamento = idDepartamento;
    this.IdLocalidad = idLocalidad;
    this.idLinea = idLinea;
    this.idEstadoPrestamo = idEstadoPrestamo;


  }
}
