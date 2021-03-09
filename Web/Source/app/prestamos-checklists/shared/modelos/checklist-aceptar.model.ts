export class ChecklistAceptarModel {
  public idFormularioLinea: number;
  public deshabilitar: boolean;
  public idEtapa: number;

  constructor(idFormularioLinea?: number, deshabilitar?: boolean, idEtapa?: number) {
    this.idFormularioLinea = idFormularioLinea;
    this.deshabilitar = deshabilitar;
    this.idEtapa = idEtapa;
  }
}
