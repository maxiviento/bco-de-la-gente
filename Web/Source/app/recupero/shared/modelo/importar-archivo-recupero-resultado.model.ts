export class ImportarArchivoRecuperoResultado {
  public cantTotal: number;
  public cantProc: number;
  public cantIncons: number;
  public cantEspec: number;
  public montoRecuperado: number;

  constructor(cantTotal?: number,
              cantProc?: number,
              cantIncons?: number,
              cantEspec?: number,
              montoRecuperado?: number) {
    this.cantTotal = cantTotal;
    this.cantProc = cantProc;
    this.cantIncons = cantIncons;
    this.cantEspec = cantEspec;
    this.montoRecuperado= montoRecuperado;
  }
}
