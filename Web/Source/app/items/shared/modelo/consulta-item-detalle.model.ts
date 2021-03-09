export class ConsultaItemDetalle {

  public id: number;
  public nombre: string;
  public descripcion: string;
  public idMotivoBaja: number;
  public nombreMotivoBaja: string;
  public fechaBaja: Date;
  public fechaUltimaModificacion: Date;
  public idUsuario: number;
  public cuilUsuario: string;


  constructor(id?: number,
              nombre?: string,
              descripcion?: string,
              idMotivoBaja?: number,
              nombreMotivoBaja?: string,
              fechaBaja?: Date,
              fechaUltimaModificacion?: Date,
              idUsuario?: number,
              cuilUsuario?: string) {
    this.id = id;
    this.nombre = nombre;
    this.descripcion = descripcion;
    this.idMotivoBaja = idMotivoBaja;
    this.nombreMotivoBaja = nombreMotivoBaja;
    this.fechaBaja = fechaBaja;
    this.fechaUltimaModificacion = fechaUltimaModificacion;
    this.idUsuario = idUsuario;
    this.cuilUsuario = cuilUsuario;
  }
}

