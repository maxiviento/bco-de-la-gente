import { MotivoRechazo } from './motivo-rechazo';
export class MotivoRechazoComando {
  public motivosRechazo: MotivoRechazo [];
  public numeroCaja: string;
  public observacion: string;

  constructor(motivosRechazo?: MotivoRechazo[], numeroCaja?: string, observacion?: string) {
    this.motivosRechazo = motivosRechazo;
    this.numeroCaja = numeroCaja;
    this.observacion = observacion;
  }
}
