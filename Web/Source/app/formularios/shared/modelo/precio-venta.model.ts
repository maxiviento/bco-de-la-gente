import { Costo } from './costo.model';

export class PrecioVenta {
  public unidadesEstimadas: number;
  public producto: string;
  public costos: Costo[];
  public gananciaEstimada: number;
  public idProducto: number;

  constructor(unidadesEstimadas?: number,
              producto?: string,
              costos?: Costo[],
              gananciaEstimada?: number,
              idProducto?: number) {
    this.unidadesEstimadas = unidadesEstimadas;
    this.producto = producto;
    this.costos = costos;
    this.gananciaEstimada = gananciaEstimada;
    this.idProducto = idProducto;
  }
}
