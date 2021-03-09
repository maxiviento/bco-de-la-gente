export class FormulariosSeleccionadosAdendaResultado {
  public linea: string;
  public nombre: string;
  public apellido: string;
  public nroDocumento: string;
  public cuil: string;
  public departamento: string;
  public localidad: string;
  public nroPrestamo: number;
  public nroFormulario: number;
  public estadoPrestamo: string;
  public montoPrestamo: string;
  public estadoFormulario: string;

  constructor(linea?: string,
              nombre?: string,
              apellido?: string,
              nroDocumento?: string,
              cuil?: string,
              departamento?: string,
              localidad?: string,
              nroPrestamo?: number,
              nroFormulario?: number,
              estadoPrestamo?: string,
              montoPrestamo?: string,
              estadoFormulario?: string) {
    this.linea = linea;
    this.nombre = nombre;
    this.apellido = apellido;
    this.nroDocumento = nroDocumento;
    this.cuil = cuil;
    this.departamento = departamento;
    this.localidad = localidad;
    this.nroPrestamo = nroPrestamo;
    this.nroFormulario = nroFormulario;
    this.estadoFormulario = estadoFormulario;
    this.estadoPrestamo = estadoPrestamo;
    this.montoPrestamo = montoPrestamo;
  }
}
