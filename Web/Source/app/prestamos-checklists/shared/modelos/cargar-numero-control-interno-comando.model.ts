export class CargarNumeroControlInternoComando {
  public idFormulario: number;
  public numeroSticker: string;

  constructor(idFormulario?: number,
              numeroSticker?: string) {
    this.idFormulario = idFormulario;
    this.numeroSticker = numeroSticker;
  }
}
