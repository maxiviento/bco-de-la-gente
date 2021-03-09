export class AgregarPrestamoLoteComando {
  public idLote: number;
  public idsPrestamo: string;
  public idMonto: number;
  public monto: number;

  constructor(idLote?: number,
              idsPrestamo?: string,
              idMonto?: number,
              monto?: number) {
    this.idLote = idLote;
    this.idsPrestamo = idsPrestamo;
    this.idMonto = idMonto;
    this.monto = monto;
  }
}
