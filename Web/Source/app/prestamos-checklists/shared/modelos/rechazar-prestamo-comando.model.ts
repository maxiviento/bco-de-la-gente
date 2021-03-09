export class RechazarPrestamoComando {
  public idPrestamo: number;
  public motivosRechazo: any[];
  public observaciones: string;
  public numeroCaja: string;

  constructor(idPrestamo?: number,
              motivosRechazo?: any,
              numeroCaja?: string,
              observaciones?: string) {
    this.idPrestamo = idPrestamo;
    this.motivosRechazo = motivosRechazo;
    this.numeroCaja = numeroCaja;
    this.observaciones = observaciones;
  }
}
