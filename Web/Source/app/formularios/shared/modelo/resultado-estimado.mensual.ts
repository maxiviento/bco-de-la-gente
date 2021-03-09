export class ResultadoEstimadoMensual {
  public productoVenta: string;
  public cantidadVenta: number;
  public precioVenta: number;
  public ingresoTotal: number;
  public gasto: string;
  public cantidadGasto: number;
  public importeGasto: number;
  public gastoTotal: number;


  constructor(productoVenta?: string,
              cantidadVenta?: number,
              precioVenta?: number,
              ingresoTotal?: number,
              gasto?: string,
              cantidadGasto?: number,
              importeGasto?: number,
              gastoTotal?: number) {

    this.productoVenta = productoVenta;
    this.cantidadVenta = cantidadVenta;
    this.precioVenta = precioVenta;
    this.ingresoTotal = ingresoTotal;
    this.gasto = gasto;
    this.cantidadGasto = cantidadGasto;
    this.importeGasto = importeGasto;
    this.gastoTotal = gastoTotal;
  }
}
