export class RechazarParametroTablaDefinida {
  public idParametro: number;
  public idMotivoRechazo: number;
  public observaciones: string;

  constructor(idParametro?: number,
              idMotivoRechazo?: number,
              observaciones?: string) {
    this.idParametro = idParametro;
    this.idMotivoRechazo = idMotivoRechazo;
    this.observaciones = observaciones;
  }
}
