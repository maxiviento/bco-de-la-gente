import { FormArray, FormGroup } from '@angular/forms';
export class ItemConfiguracionChecklist {
  public nombreItem: string;
  public nombreEtapa: string;
  public nombreArea: string;
  public idItem: number;
  public idItemPadre: number;
  public esDeSolicitante: boolean;
  public esDeGarante: boolean;
  public esDeChecklist: boolean;
  public idArea: number;
  public idEtapa: number;
  public nroOrden: number;
  public seleccionado: boolean;
  public idRequisito: number;
  public idLinea: number;
  public idTipoRequisito: number;
  public itemsHijos: ItemConfiguracionChecklist [];

  constructor(nombreItem?: string,
              idItem?: number,
              idItemPadre?: number,
              esDeSolicitante?: boolean,
              esDeGarante?: boolean,
              esDeChecklist?: boolean,
              idArea?: number,
              idEtapa?: number,
              nroOrden?: number,
              idRequisito?: number,
              idTipoRequisito?: number,
              nombreEtapa?: string,
              nombreArea?: string,
              seleccionado?: boolean) {
    this.nombreItem = nombreItem;
    this.nombreArea = nombreArea;
    this.nombreEtapa = nombreEtapa;
    this.idItem = idItem;
    this.idItemPadre = idItemPadre;
    this.esDeSolicitante = esDeSolicitante;
    this.esDeGarante = esDeGarante;
    this.esDeChecklist = esDeChecklist;
    this.idArea = idArea;
    this.idEtapa = idEtapa;
    this.seleccionado = seleccionado;
    this.nroOrden = nroOrden;
    this.idRequisito = idRequisito;
    this.idTipoRequisito = idTipoRequisito;
    this.itemsHijos = [];
  }

  public static construirItemConfiguracion(form: FormGroup): ItemConfiguracionChecklist {
    let item = new ItemConfiguracionChecklist();
    item.nombreItem = form.get('nombre').value;
    item.nombreArea = form.get('nombreArea').value;
    item.idItem = form.get('id').value;
    item.idItemPadre = form.get('idItemPadre').value;
    item.esDeSolicitante = form.get('esSolic').value;
    item.esDeGarante = form.get('esGarante').value;
    item.idTipoRequisito = form.get('idTipoRequisito').value;
    item.esDeChecklist = form.get('esChecklist').value;
    item.idArea = form.get('idArea').value;
    if (form.get('itemsHijos')) {
      (form.get('itemsHijos') as FormArray).controls.forEach((hijo) =>
        item.itemsHijos.push(ItemConfiguracionChecklist.construirItemConfiguracion(hijo as FormGroup)));
    }
    return item;
  }

  public static construirRequisitoConfiguracion(id: number,
                                                idTipoRequisito: number,
                                                idItem: number,
                                                nombreItem: string,
                                                itemPadre: number,
                                                idArea: number,
                                                nombreArea: string,
                                                esSolicitante: boolean,
                                                esGarante: boolean,
                                                esChecklist: boolean,
                                                itemsHijos?: ItemConfiguracionChecklist []): ItemConfiguracionChecklist {
    let requisito = new ItemConfiguracionChecklist();
    requisito.idRequisito = id;
    requisito.idItem = idItem;
    requisito.idTipoRequisito = idTipoRequisito;
    requisito.nombreItem = nombreItem;
    requisito.nombreArea = nombreArea;
    requisito.idItemPadre = itemPadre;
    requisito.idArea = idArea;
    requisito.esDeGarante = esGarante;
    requisito.esDeSolicitante = esSolicitante;
    requisito.esDeChecklist = esChecklist;
    requisito.itemsHijos = itemsHijos;
    return requisito;
  }
}
