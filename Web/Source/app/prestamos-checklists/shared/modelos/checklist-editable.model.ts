export class ChecklistEditableModel {
  public idFormularioLinea: number;
  public editable: boolean;

  constructor(idFormularioLinea?: number, editable?: boolean) {
    this.idFormularioLinea = idFormularioLinea;
    this.editable = editable;
  }
}
