export class BandejaPagosConsulta {
  public fechaInicioTramite: Date;
  public fechaFinTramite: Date;
  public idDepartamento: number;
  public idLocalidad: number;
  public nroPrestamoChecklist: string;
  public nroFormulario: string;
  public idsLineas: number[];
  public idOrigen: number;
  public idLugarOrigen: number;
  public fechaPago: Date;
  public idAgrupamiento: number;

  public tipoPersona: number;
  public cuil: string;
  public apellido: string;
  public nombre: string;
  public dni: string;

  public departamentoIds: string[];
  public localidadIds: string[];

  public numeroPagina: number;
  public tamañoPagina: number;
  public orderByDes: boolean;
  public columnaOrderBy: number;

  constructor(fechaInicioTramite?: Date,
              fechaFinTramite?: Date,
              idDepartamento?: number,
              idLocalidad?: number,
              nroPrestamoChecklist?: string,
              nroFormulario?: string,
              idsLineas?: number[],
              idOrigen?: number,
              idLugarOrigen?: number,
              fechaPago?: Date,
              idAgrupamiento?: number,
              tipoPersona?: number,
              cuil?: string,
              apellido?: string,
              nombre?: string,
              dni?: string,
              departamentoIds?: string[],
              localidadIds?: string[],
              numeroPagina?: number,
              tamañoPagina?: number,
              orderByDes?: boolean,
              columnaOrderBy?: number,) {

    this.fechaInicioTramite = fechaInicioTramite;
    this.fechaFinTramite = fechaFinTramite;
    this.idDepartamento = idDepartamento;
    this.idLocalidad = idLocalidad;
    this.nroPrestamoChecklist = nroPrestamoChecklist;
    this.nroFormulario = nroFormulario;
    this.idsLineas = idsLineas;
    this.idOrigen = idOrigen;
    this.fechaPago = fechaPago;
    this.idAgrupamiento = idAgrupamiento;
    this.idLugarOrigen = idLugarOrigen;
    this.tipoPersona = tipoPersona;
    this.cuil = cuil;
    this.apellido = apellido;
    this.nombre = nombre;
    this.dni = dni;
    this.departamentoIds = departamentoIds;
    this.localidadIds = localidadIds;
    this.numeroPagina = numeroPagina;
    this.tamañoPagina = tamañoPagina;
    this.orderByDes = orderByDes;
    this.columnaOrderBy = columnaOrderBy;
  }
}
