export class ModificarMotivoRechazoComando {
  public id: number;
  public nombreNuevo: string;
  public descripcionNueva: string;
  public codigoNuevo: string;
  public abreviaturaNueva: string;
  public nombreOriginal: string;
  public descripcionOriginal: string;
  public codigoOriginal: string;
  public abreviaturaOriginal: string;

  constructor(
    id?: number,
    nombreNuevo?: string,
    descripcionNueva?: string,
    codigoNuevo?: string,
    abreviaturaNueva?: string,
    nombreOriginal?: string,
    descripcionOriginal?: string,
    codigoOriginal?: string,
    abreviaturaOriginal?: string
  ) {
    this.id = id;
    this.nombreNuevo = nombreNuevo;
    this.descripcionNueva = descripcionNueva;
    this.codigoNuevo = codigoNuevo;
    this.abreviaturaNueva = abreviaturaNueva;
    this.nombreOriginal = nombreOriginal;
    this.descripcionOriginal = descripcionOriginal;
    this.codigoOriginal = codigoOriginal;
    this.abreviaturaOriginal = abreviaturaOriginal;
  }
}
