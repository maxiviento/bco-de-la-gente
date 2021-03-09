import { MotivoRechazo } from './motivo-rechazo';

export class RechazarFormularioComando {
  public idFormulario: number;
  public idMotivoBaja: number;
  public motivosRechazo: MotivoRechazo[];
  public numeroCaja: string;
  public esAsociativa: boolean;

  constructor(idFormulario?: number,
              idMotivoBaja?: number,
              motivosRechazo?: any,
              numeroCaja?: string,
              esAsociativa?: boolean) {
    this.idFormulario = idFormulario;
    this.idMotivoBaja = idMotivoBaja;
    this.motivosRechazo = motivosRechazo;
    this.numeroCaja = numeroCaja;
    this.esAsociativa = esAsociativa;
  }
}
