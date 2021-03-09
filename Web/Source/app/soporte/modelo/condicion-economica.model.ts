export class CondicionEconomica {
  public objeto: string;
  public cuit: string;
  public baseImponible: number;
  public porcentaje: number;

  constructor() {
    this.objeto = undefined;
    this.cuit = undefined;
    this.baseImponible = undefined;
    this.porcentaje = undefined;
  }
}
