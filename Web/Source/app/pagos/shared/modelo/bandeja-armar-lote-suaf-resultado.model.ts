export class BandejaArmarLoteSuafResultado {
  public id: number;
  public idPrestamo: number;
  public linea: string;
  public departamento: string;
  public localidad: string;
  public origen: string;
  public nroPrestamo: number;
  public nroFormulario: number;
  public montoOtorgado: number;
  public fechaPedido: Date;
  public seleccionado: boolean;
  public devengado: string;
  public devengadoNuevo: string;
  public fechaDevengado: Date;
  public apellidoYNombre: string;

  constructor(id?: number,
              idPrestamo?: number,
              linea?: string,
              departamento?: string,
              localidad?: string,
              origen?: string,
              nroPrestamo?: number,
              nroFormulario?: number,
              montoOtorgado?: number,
              fechaPedido?: Date,
              seleccionado?: boolean,
              devengado?: string,
              devengadoNuevo?: string,
              fechaDevengado?: Date,
              apellidoYNombre?: string) {
    this.id = id;
    this.idPrestamo = idPrestamo;
    this.linea = linea;
    this.departamento = departamento;
    this.localidad = localidad;
    this.origen = origen;
    this.nroPrestamo = nroPrestamo;
    this.nroFormulario = nroFormulario;
    this.montoOtorgado = montoOtorgado;
    this.fechaPedido = fechaPedido;
    this.seleccionado = seleccionado;
    this.devengado = devengado;
    this.devengadoNuevo = devengadoNuevo;
    this.fechaDevengado = fechaDevengado;
    this.apellidoYNombre = apellidoYNombre;
  }
}
