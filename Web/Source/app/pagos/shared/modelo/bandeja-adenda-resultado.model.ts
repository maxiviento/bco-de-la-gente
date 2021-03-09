export class BandejaAdendaResultado {
  public id: number;
  public linea: string;
  public departamento: string;
  public localidad: string;
  public nroPrestamo: number;
  public montoOtorgado: number;
  public nroFormulario: number;
  public apellidoYNombre: string;
  public agregado: boolean;

  constructor(id?: number,
              linea?: string,
              departamento?: string,
              localidad?: string,
              nroPrestamo?: number,
              montoOtorgado?: number,
              nroFormulario?: number,
              apellidoYNombre?: string,
              agregado?: boolean) {
    this.id = id;
    this.linea = linea;
    this.departamento = departamento;
    this.localidad = localidad;
    this.nroPrestamo = nroPrestamo;
    this.montoOtorgado = montoOtorgado;
    this.nroFormulario = nroFormulario;
    this.apellidoYNombre = apellidoYNombre;
    this.agregado = agregado;
  }
}
