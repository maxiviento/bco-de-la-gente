export class ItemUsuario {
  public id: number;
  public nombre: string;
  public apellido: string;
  public cuil: number;
  public perfilId: number;
  public nombrePerfil: string;
  public fechaAlta: Date;
  public fechaBaja: Date;
  public motivoBajaId: number;
  public nombreMotivoBaja: string;
  public activo: boolean;
  public sistema: boolean;

  constructor(id?: number,
              nombre?: string,
              apellido?: string,
              cuil?: number,
              perfilId?: number,
              nombrePerfil?: string,
              fechaAlta?: Date,
              fechaBaja?: Date,
              motivoBajaId?: number,
              nombreMotivoBaja?: string,
              activo?: boolean,
              sistema?: boolean) {

    this.id = id;
    this.nombre = nombre;
    this.apellido = apellido;
    this.cuil = cuil;
    this.perfilId = perfilId;
    this.nombrePerfil = nombrePerfil;
    this.fechaAlta = fechaAlta;
    this.fechaBaja = fechaBaja;
    this.motivoBajaId = motivoBajaId;
    this.nombreMotivoBaja = nombreMotivoBaja;
    this.activo = activo;
    this.sistema = sistema;
  }
}
