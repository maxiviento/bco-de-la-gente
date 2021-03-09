import {JsonProperty} from '../../../shared/map-utils';
import {TipoItem} from "../../../items/shared/modelo/tipo-item.model";

export class RequisitosResultado {

  public nombreLinea: string;
  public idTipoItem: number;
  public idItem: number;
  public nombreTipoItem: string;
  public nombreItem: string;
  @JsonProperty({clazz: TipoItem})
  public tiposItems: TipoItem[] = [];


  constructor(nombreLinea?: string,
              idTipoItem?: number,
              idItem?: number,
              nombreTipoItem?: string,
              nombreItem?: string,
              tiposItems?: TipoItem[]) {

    this.nombreLinea = nombreLinea;
    this.idTipoItem = idTipoItem;
    this.idItem = idItem;
    this.nombreTipoItem = nombreTipoItem;
    this.nombreItem = nombreItem;
    this.tiposItems = tiposItems;
  }
}
