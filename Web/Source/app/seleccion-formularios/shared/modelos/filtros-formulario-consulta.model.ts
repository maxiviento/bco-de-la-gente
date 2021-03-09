export class FiltrosFormularioConsulta {
  public idLote: number;
  public cuil: string;
  public dni: string;
  public nroPrestamo: number;
  public nroFormulario: number;
  public idDepartamento: number;
  public idPrestamoItem: number;
  public idLocalidad: number;
  public consulta: number;
  public numeroPagina: number;
  public tamañoPagina: number;
  public idFormularioLinea: number;

  constructor() {
    this.idLote = undefined;
    this.cuil = undefined;
    this.dni = undefined;
    this.nroPrestamo = undefined;
    this.nroFormulario = undefined;
    this.idDepartamento = undefined;
    this.idLocalidad = undefined;
    this.consulta = undefined;
    this.numeroPagina = undefined;
    this.tamañoPagina = undefined;
    this.idFormularioLinea = undefined;
    this.idPrestamoItem = undefined;
  }
}
