export class CargaDevengadoComandoModel {
  public nroFormulario: number;
  public idPrestamo: number;
  public devengado: string;
  public idFormulario: number;

  constructor(nroFormulario?: number,
              idPrestamo?: number,
              devengado?: string,
              idFormulario?: number) {
    this.nroFormulario = nroFormulario;
    this.idPrestamo = idPrestamo;
    this.devengado = devengado;
    this.idFormulario = idFormulario;
  }
}
