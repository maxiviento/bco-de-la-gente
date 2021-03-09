export class FiltrosUsuarios {
  public cuil: string;
  public perfilId: string;
  public incluyeBajas: string;
  public numeroPagina: number;

  constructor(cuil?: string, perfilId?: string, incluyeBajas?: string,numeroPagina?: number) {
    this.cuil = cuil;
    this.perfilId = perfilId;
    this.incluyeBajas = incluyeBajas;
    this.numeroPagina = numeroPagina;
  }
}
