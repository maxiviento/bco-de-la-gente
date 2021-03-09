import { ItemConfiguracionChecklist } from './item-configuracion-checklist.model';
export class EtapaConfiguracionChecklist {
  public id?: number;
  public nombre?: string;
  public itemsEtapa: ItemConfiguracionChecklist [];

  constructor() {
    this.id = undefined;
    this.nombre = undefined;
    this.itemsEtapa = undefined;
  }

  public static construir(id?: number, nombre?: string, itemsEtapa?: ItemConfiguracionChecklist[]): EtapaConfiguracionChecklist {
    let etapa = new EtapaConfiguracionChecklist();
    etapa.id = id;
    etapa.nombre = nombre;
    etapa.itemsEtapa = itemsEtapa;
    return etapa;
  }
}
