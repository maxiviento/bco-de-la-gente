export class EtapaEstadoLinea{
  public id: number;
  public orden: number;
  public idEtapaActual: number;
  public idEstadoActual: number;
  public idEtapaSiguiente: number;
  public idEstadoSiguiente: number;
  public idLineaPrestamo: number;

  constructor(id?: number,
              orden?: number,
              idEtapaActual?: number,
              idEstadoActual?: number,
              idEtapaSiguiente?: number,
              idEstadoSiguiente?: number,
              idLineaPrestamo?: number) {
    this.id = id;
    this.orden = orden;
    this.idEtapaActual = idEtapaActual;
    this.idEstadoActual = idEstadoActual;
    this.idEtapaSiguiente = idEtapaSiguiente;
    this.idEstadoSiguiente = idEstadoSiguiente;
    this.idLineaPrestamo = idLineaPrestamo;
  }
}
