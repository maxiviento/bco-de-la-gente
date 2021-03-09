export class VigenciaParametroConsulta {
  public id: number;
  public fecha: Date;

  constructor(id?: number, fecha?: Date) {
    this.id = id;
    this.fecha = fecha;
  }
}
