export class EstimaCantClientes {
  public estima: boolean;
  public cantidad: number;

  constructor(estima?: boolean,
              cantidad?: number) {
    this.estima = estima;
    this.cantidad = cantidad;
  }
}
