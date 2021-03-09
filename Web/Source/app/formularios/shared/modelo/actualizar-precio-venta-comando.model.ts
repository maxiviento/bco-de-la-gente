import { Costo } from './costo.model';

export class ActualizarPrecioVentaComando {
  public unidadesEstimadas: number;
  public producto: string;
  public costos: Costo[];
  public idEmprendimiento: number;
  public precioVenta: number;
  public costosAEliminar: number[];
  public gananciaEstimada: number;
  public idProducto: number;

  constructor(unidadesEstimadas?: number,
              producto?: string,
              costos?: Costo[],
              idEmprendimiento?: number,
              precioVenta?: number,
              costosAEliminar?: number[],
              ganancia?: number,
              idProducto?: number) {
    this.unidadesEstimadas = unidadesEstimadas;
    this.producto = producto;
    this.costos = costos;
    this.idEmprendimiento = idEmprendimiento;
    this.precioVenta = precioVenta;
    this.costosAEliminar = costosAEliminar;
    this.gananciaEstimada = ganancia;
    this.idProducto = idProducto;
  }
}
