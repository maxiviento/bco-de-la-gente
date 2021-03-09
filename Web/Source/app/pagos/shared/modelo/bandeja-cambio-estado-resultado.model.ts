export class BandejaCambioEstadoResultado {
  public idFormulario: number;
  public nroFormulario: number;
  public nroPrestamo: number;
  public linea: string;
  public numeroPagina: number;
  public tamañoPagina: number;
  public estadoFormulario: string;
  public estadoPrestamo: string;
  public apellido: string;
  public nombre: string;
  public dni: string;
  public fechaFormulario: Date;
  public localidad: string;
  public elementoPago: string;

  constructor(idFormulario?: number,
              nroFormulario?: number,
              nroPrestamo?: number,
              linea?: string,
              estadoFormulario?: string,
              estadoPrestamo?: string,
              apellido?: string,
              nombre?: string,
              dni?: string,
              fechaFormulario?: Date,
              localidad?: string,
              elementoPago?: string) {
    this.idFormulario = idFormulario;
    this.nroFormulario = nroFormulario;
    this.nroPrestamo = nroPrestamo;
    this.linea = linea;
    this.estadoFormulario = estadoFormulario;
    this.estadoPrestamo = estadoPrestamo;
    this.apellido = apellido;
    this.nombre = nombre;
    this.dni = dni;
    this.fechaFormulario = fechaFormulario;
    this.localidad = localidad;
    this.elementoPago = elementoPago;
  }
}
