import { MotivoRechazo } from './motivo-rechazo';

export class RegistrarRechazoReactivacionPrestamo {
  public idPrestamo: number;
  public numeroCaja: string;
  public motivosRechazo: MotivoRechazo[];

  constructor(idPrestamo?: number,
              numeroCaja?: string,
              motivoRechazo?: MotivoRechazo[]) {
    this.idPrestamo = idPrestamo;
    this.numeroCaja = numeroCaja;
    this.motivosRechazo = motivoRechazo;
  }
}
