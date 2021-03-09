export class MotivoDestino {
  public id: number;
  public nombre: string;
  public descripcion: string;
  public idUsuarioAlta: number;
  public cuilUsuarioAlta: string;
  public idMotivoBaja: number;
  public nombreMotivoBaja: string;
  public fechaUltimaModificacion: Date;
  public idUsuarioUltimaModificacion: number;
  public cuilUsuarioUltimaModificacion: string;

  constructor(id?: number, nombre?: string, descripcion?: string) {
    this.id = id;
    this.nombre = nombre;
    this.descripcion = descripcion;
    this.idUsuarioAlta = undefined;
    this.cuilUsuarioAlta = undefined;
    this.idMotivoBaja = undefined;
    this.nombreMotivoBaja = undefined;
    this.fechaUltimaModificacion = undefined;
    this.idUsuarioUltimaModificacion = undefined;
    this.cuilUsuarioUltimaModificacion = undefined;
  }

  public estaDadaDeBaja(): boolean {
    return this.idMotivoBaja != null;
  }
}
