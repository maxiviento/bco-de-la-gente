export class BandejaPrestamoResultado {
  public nroFormulario: string;
  public fechaAltaPrestamo: Date;
  public nroPrestamo: string;
  public nroLinea: string;
  public nombreYApellidoSolicitante: string;
  public cuil: string;
  public estadoPrestamo: string;
  public estadoFormulario: string;
  public origen: string;
  public nroStricker: string;
  public id: number;
  public esAsociativa: boolean;
  public idEstadoPrestamo: number;
  public idEstadoPrestamoAnt: number;
  public idFormularioLinea: number;
  public numeroCaja: string;
  public esApoderado: boolean;
  public montoOtorgado: number;
  public idEstadoFormulario: number;

  constructor(NroFormulario?: string,
              NroPrestamo?: string,
              NroLinea?: string,
              NombreYApellidoSolicitante?: string,
              Cuil?: string,
              EstadoPrestamo?: string,
              EstadoFormulario?: string,
              Origen?: string,
              NroStricker?: string,
              Id?: number,
              esAsociativas?: boolean,
              idEstadoPrestamo?: number,
              idEstadoPrestamoAnt?: number,
              idFormularioLinea?: number,
              numeroCaja?: string,
              fechaAltaPrestamo?: Date,
              esApoderado?: boolean,
              montoOtorgado?: number,
              idEstadoFOrmulario?: number) {
    this.id = Id;
    this.nroFormulario = NroFormulario;
    this.nroPrestamo = NroPrestamo;
    this.nroLinea = NroLinea;
    this.nombreYApellidoSolicitante = NombreYApellidoSolicitante;
    this.cuil = Cuil;
    this.estadoPrestamo = EstadoPrestamo;
    this.estadoFormulario = EstadoFormulario;
    this.origen = Origen;
    this.nroStricker = NroStricker;
    this.esAsociativa = esAsociativas;
    this.idEstadoPrestamo = idEstadoPrestamo;
    this.idEstadoPrestamoAnt = idEstadoPrestamoAnt;
    this.idFormularioLinea = idFormularioLinea;
    this.numeroCaja = numeroCaja;
    this.esApoderado = esApoderado;
    this.fechaAltaPrestamo = fechaAltaPrestamo;
    this.montoOtorgado = montoOtorgado;
    this.idEstadoFormulario = idEstadoFOrmulario;
  }
}
