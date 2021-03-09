export class VerImportacionArchivo {
  public nroLinea: number;
  public motivoRechazo: string;

  constructor(nroLinea?: number,
              motivoRechazo?: string) {
    this.nroLinea = nroLinea;
    this.motivoRechazo = motivoRechazo;
  }
}
