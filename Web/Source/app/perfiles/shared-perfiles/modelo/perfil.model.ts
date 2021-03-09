export class Perfil {

  public nombre: string;
  public fechaAlta: Date;
  public fechaBaja: Date;
  public funcionalidades: string[];
  public motivoBaja: string;

  constructor(nombre?: string,
              fechaAlta?: Date,
              fechaBaja?: Date,
              funcionalidades?: string[],
              motivoBaja?: string) {
    this.nombre = nombre;
    this.fechaAlta = fechaAlta;
    this.fechaBaja = fechaBaja;
    this.funcionalidades = funcionalidades;
    this.motivoBaja = motivoBaja;
  }
}
