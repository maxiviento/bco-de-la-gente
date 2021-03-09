export class BandejaCargaNumeroControlInterno {
  public idFormulario: number;
  public numero: number;
  public apellidoNombreSolicitante: string;
  public estado: string;
  public nroSticker: string;
  public cuilSolicitante: string;

  constructor(idFormulario?: number,
              numero?: number,
              apellidoNombreSolicitante?: string,
              estado?: string,
              cuilSolicitante?: string) {
    this.idFormulario = idFormulario;
    this.numero = numero;
    this.apellidoNombreSolicitante = apellidoNombreSolicitante;
    this.estado = estado;
    this.nroSticker = '';
    this.cuilSolicitante = cuilSolicitante;
  }
}
