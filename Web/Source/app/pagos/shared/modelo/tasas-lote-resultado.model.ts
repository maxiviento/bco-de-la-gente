export class TasasLoteResultado {
  public iva: number;
  public comision: number;

  constructor(iva?: number,
              comision?: number) {
    this.iva = iva;
    this.comision = comision;
  }
}
