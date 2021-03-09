export class Convenio {
  public id: number;
  public nombre: string;
  public idTipoConvenio: number; // 1 -> Pagos, 2 -> Recupero

  constructor(id?: number, nombre?: string, idTipoConvenio?: number) {
    this.id = id;
    this.nombre = nombre;
    this.idTipoConvenio = idTipoConvenio;
  }
}
