export class BandejaFormularioResultado {
  public id: number;
  public nroFormulario: string;
  public localidad: string;
  public linea: string;
  public puedeConformarPrestamo: boolean;
  public apellidoNombreSolicitante: string;
  public cuilSolicitante: string;
  public fechaEnvio: Date;
  public origen: string;
  public estado: string;
  public idEstado: number;
  public esAsociativa: boolean;
  public numeroCaja: string;
  public esApoderado: boolean;
  public fechaInicio: Date;
  public fechaSeguimiento: Date;

  constructor(id?: number,
              nroFormulario?: string,
              localidad?: string,
              linea?: string,
              apellidoNombreSolicitante?: string,
              cuilSolicitante?: string,
              fechaEnvio?: Date,
              origen?: string,
              estado?: string,
              puedeConformarPrestamo?: boolean,
              idEstado?: number,
              esAsociativa?: boolean,
              numeroCaja?: string,
              esApoderado?: boolean,
              fechaInicio?: Date,
              fechaSeguimiento?: Date) {
    this.id = id;
    this.nroFormulario = nroFormulario;
    this.localidad = localidad;
    this.linea = linea;
    this.puedeConformarPrestamo = puedeConformarPrestamo;
    this.apellidoNombreSolicitante = apellidoNombreSolicitante;
    this.cuilSolicitante = cuilSolicitante;
    this.fechaEnvio = fechaEnvio;
    this.origen = origen;
    this.estado = estado;
    this.idEstado = idEstado;
    this.esAsociativa = esAsociativa;
    this.numeroCaja = numeroCaja;
    this.esApoderado = esApoderado;
    this.fechaInicio = fechaInicio;
    this.fechaSeguimiento = fechaSeguimiento;
  }
}
