export class BandejaPagosResultado {
  public id: number;
  public linea: string;
  public departamento: string;
  public localidad: string;
  public origen: string;
  public cantFormularios: number;
  public nroPrestamo: number;
  public montoOtorgado: number;
  public fechaPedido: Date;
  public seleccionado: boolean;
  public nroFormulario: number;
  public apellidoYNombre: string;

  constructor(id?: number,
              linea?: string,
              departamento?: string,
              localidad?: string,
              origen?: string,
              cantFormularios?: number,
              nroPrestamo?: number,
              montoOtorgado?: number,
              fechaPedido?: Date,
              seleccionado?: boolean,
              nroFormulario?: number,
              apellidoYNombre?: string) {
    this.id = id;
    this.linea = linea;
    this.departamento = departamento;
    this.localidad = localidad;
    this.origen = origen;
    this.cantFormularios = cantFormularios;
    this.nroPrestamo = nroPrestamo;
    this.montoOtorgado = montoOtorgado;
    this.fechaPedido = fechaPedido;
    this.seleccionado = seleccionado;
    this.nroFormulario = nroFormulario;
    this.apellidoYNombre = apellidoYNombre;
  }
}
