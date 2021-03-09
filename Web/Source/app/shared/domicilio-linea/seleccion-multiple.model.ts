export class SeleccionMultiple {
  public id: number;
  public name: string;
  public parentId: string;
  public isLabel: boolean;

  constructor(id?: number, name?: string, parentId?: string, isLabel?: boolean) {
    this.name = name;
    this.id = id;
    this.parentId = parentId;
    this.isLabel = isLabel;
  }
}
