export class FormularioPrestamo {
  public id: number;
  public idLinea: number;
  public nombreLinea: string;
  public nombre: string;
  public cantidad: number;
  public fechaBGE: Date;
  public origen: string;
  public idFormulario: number;
  public idEstado: number;
  public minIntegrantes: number;
  public maxIntegrantes: number;
  public esApoderado: number;
  public nroFormulario: number;
  public nroDocumento: string;
  public apellido: string;
  public cuil: string;

  constructor(id?: number,
              idLinea?: number,
              nombreLinea?: string,
              nombre?: string,
              cantidad?: number,
              fechaBGE?: Date,
              origen?: string,
              idFormulario?: number,
              idEstado?: number,
              cantidadMinimaIntegrantes?: number,
              cantidadMaximaIntegrantes?: number,
              esApoderado?: number,
              nroFormulario?: number,
              apellido?: string,
              cuil?: string) {
    this.id = id;
    this.idLinea = idLinea;
    this.nombreLinea = nombreLinea;
    this.nombre = nombre;
    this.cantidad = cantidad;
    this.fechaBGE = fechaBGE;
    this.origen = origen;
    this.idFormulario = idFormulario;
    this.idEstado = idEstado;
    this.minIntegrantes = cantidadMinimaIntegrantes;
    this.maxIntegrantes = cantidadMaximaIntegrantes;
    this.esApoderado = esApoderado;
    this.nroFormulario = nroFormulario;
    this.apellido = apellido;
    this.cuil = cuil;
  }
}
