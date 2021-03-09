import { JsonProperty } from '../../../shared/map-utils';
import { Item } from './item.model';

export class ItemTipoItem {
  public id: number;
  public nombre: string;
  @JsonProperty({clazz: Item})
  public items: Item [];

  constructor() {
    this.id = undefined;
    this.nombre = undefined;
    this.items = [];
  }
}
