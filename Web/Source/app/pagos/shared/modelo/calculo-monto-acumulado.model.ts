export class CalculoMontoAcumulado {
  public montoAcumulado: number;
  public cantidadPrestamos: number;
  public montoAcumuladoIvaComision: number;

  constructor(montoAcumulado?: number,
              cantidadPrestamos?: number,
              montoAcumuladoIvaComision?: number) {
    this.montoAcumulado = montoAcumulado;
    this.cantidadPrestamos = cantidadPrestamos;
    this.montoAcumuladoIvaComision = montoAcumuladoIvaComision;
  }
}
