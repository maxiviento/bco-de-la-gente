export class PatrimonioSolicitante {
  public inmueblePropio: boolean;
  public valorInmueble: number;
  public vehiculoPropio: boolean;
  public cantVehiculos: number;
  public modeloVehiculos: string;
  public valorVehiculos: number;
  public valorInmueblesMasVehiculos: number;
  public valorDeudas: number;

  constructor(inmueblePropio?: boolean,
              valorInmueble?: number,
              vehiculoPropio?: boolean,
              cantVehiculos?: number,
              modeloVehiculos?: string,
              valorVehiculos?: number,
              valorInmueblesMasVehiculos?: number,
              valorDeudas?: number) {
    this.inmueblePropio = inmueblePropio;
    this.valorInmueble = valorInmueble;
    this.vehiculoPropio = vehiculoPropio;
    this.cantVehiculos = cantVehiculos;
    this.modeloVehiculos = modeloVehiculos;
    this.valorVehiculos = valorVehiculos;
    this.valorInmueblesMasVehiculos = valorInmueblesMasVehiculos;
    this.valorDeudas = valorDeudas;
  }
}
