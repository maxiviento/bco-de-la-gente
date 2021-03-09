export class SituacionDomicilioIntegrante {
  public id: number;
  public idGrupoUnico: number;
  public apellidoNombre: string;
  public numeroDocumento: string;
  public sexo: string;
  public vigente: string;
  public hayRechazo: string;
  public montoCuotasVencidas: number;
  public montoCuotasAVencer: number;
  public montoCuotasVencidasAsociativa: number;
  public montoCuotasAVencerAsociativa: number;

  constructor(idGrupoUnico?: number,
              apellidoNombre?: string,
              numeroDocumento?: string,
              vigente?: string,
              hayRechazo?: string,
              montoCuotasVencidas?: number,
              montoCuotasAVencer?: number,
              montoCuotasVencidasAsociativa?: number,
              montoCuotasAVencerAsociativa?: number) {
    this.idGrupoUnico = idGrupoUnico;
    this.apellidoNombre = apellidoNombre;
    this.numeroDocumento = numeroDocumento;
    this.vigente = vigente;
    this.hayRechazo = hayRechazo;
    this.montoCuotasVencidas = montoCuotasVencidas;
    this.montoCuotasAVencer = montoCuotasAVencer;
    this.montoCuotasVencidasAsociativa = montoCuotasVencidasAsociativa;
    this.montoCuotasAVencerAsociativa = montoCuotasAVencerAsociativa;
  }
}
