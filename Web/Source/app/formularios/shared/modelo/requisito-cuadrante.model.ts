export class RequisitoCuadrante {
  public descripcion: string;
  public esSolicitante: boolean;
  public esGarante: boolean;

  public constructor(descripcion?: string,
                     esSolicitante?: boolean,
                     esGarante?: boolean) {
    this.descripcion = descripcion;
    this.esSolicitante = esSolicitante;
    this.esGarante = esGarante;
  }
}
