export class ProvidenciaComando {
  public idFormulario: number;
  public fecha: Date;
  public fechaManual: boolean;
  public idLote: number;
  public fechaAprovacionMasiva: boolean;

  constructor(idFormulario?: number,
              fecha?: Date,
              fechaManual?: boolean,
              idLote?: number,
              fechaAprovacionMasiva?: boolean) {
    this.idFormulario = idFormulario;
    this.fecha = fecha;
    this.fechaManual = fechaManual;
    this.idLote = idLote;
    this.fechaAprovacionMasiva = fechaAprovacionMasiva;
  }
}
