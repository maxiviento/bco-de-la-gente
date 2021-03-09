// Condiciones del prestamo definidas en el detalle de la linea
export class CondicionesPrestamo {
  public montoMaximo: number;
  public cantidadMaximaCuotas: number;

  public constructor(montoMaximo?: number,
                     cantidadMaximaCuotas?: number) {
    this.montoMaximo = montoMaximo;
    this.cantidadMaximaCuotas = cantidadMaximaCuotas;
  }
}
