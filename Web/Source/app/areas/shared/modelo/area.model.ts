export class Area {
  public id?: number;
  public nombre?: string;
  public descripcion?: string;
  public idUsuarioAlta?: number;
  public cuilUsuarioAlta?: string;
  public idMotivoBaja?: number;
  public nombreMotivoBaja?: string;
  public fechaUltimaModificacion?: Date;
  public idUsuarioUltimaModificacion?: number;
  public cuilUsuarioUltimaModificacion?: string;
  public idEtapa?: number;

  constructor(id?: number,
              nombre?: string,
              descripcion?: string,
              idUsuarioAlta?: number,
              cuilUsuarioAlta?: string,
              idMotivoBaja?: number,
              nombreMotivoBaja?: string,
              fechaUltimaModificacion?: Date,
              idUsuarioUltimaModificacion?: number,
              cuilUsuarioUltimaModificacion?: string) {
    this.id = id;
    this.nombre = nombre;
    this.descripcion = descripcion;
    this.idUsuarioAlta = idUsuarioAlta;
    this.cuilUsuarioAlta = cuilUsuarioAlta;
    this.idMotivoBaja = idMotivoBaja;
    this.nombreMotivoBaja = nombreMotivoBaja;
    this.fechaUltimaModificacion = fechaUltimaModificacion;
    this.idUsuarioUltimaModificacion = idUsuarioUltimaModificacion;
    this.cuilUsuarioUltimaModificacion = cuilUsuarioUltimaModificacion;
    this.idEtapa = undefined;
  }

  public static construirAreaChecklist(id: number, nombre: string, idEtapa: number): Area {
    let area = new Area();
    area.id = id;
    area.nombre = nombre;
    area.idEtapa = idEtapa;
    return area;
  }

  public estaDadaDeBaja(): boolean {
    return this.idMotivoBaja != null;
  }
}
