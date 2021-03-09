export class GenerarAdendaComando {
  public nroDetalle: number;
  public comando: string;
  constructor(nroDetalle?: number,
              comando?: string) {
    this.nroDetalle = nroDetalle;
    this.comando = comando;
  }
}
