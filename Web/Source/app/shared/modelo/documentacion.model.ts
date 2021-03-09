export class Documentacion {
  public documento?: any;
  public idItem?: number;
  public idFormularioLinea?: number;

  constructor(documento?: any, idItem?: number, idFormularioLinea?: number) {
    this.documento = documento;
    this.idItem = idItem;
    this.idFormularioLinea = idFormularioLinea;
  }
}
