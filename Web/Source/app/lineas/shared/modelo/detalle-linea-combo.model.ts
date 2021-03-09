export class DetalleLineaCombo {
  public idLineaPrestamo: number;
  public idDetalleLinea: number;
  public visualizacion: string;

  constructor(idLineaPrestamo?: number,
              idDetalleLinea?: number,
              visualizacion?: string) {
    this.idLineaPrestamo = idLineaPrestamo;
    this.idDetalleLinea = idDetalleLinea;
    this.visualizacion = visualizacion;
  }
}
