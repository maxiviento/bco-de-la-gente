export class ItemPerfiles {
  public id: number;
  public nombre: string;
  public fechaBaja: Date;
  public fechaModificacion: Date;
  public permiteBaja: boolean;
  public permiteModificacion: boolean;

  constructor(id?: number,
              nombre?: string,
              fechaBaja?: Date,
              fechaModificacion?: Date,
              permiteModificacion?: boolean,
              permiteBaja?: boolean) {
    this.id = id;
    this.nombre = nombre;
    this.fechaBaja = fechaBaja;
    this.fechaModificacion = fechaModificacion;
    this.permiteBaja = permiteBaja;
    this.permiteModificacion = permiteModificacion;
  }
}
