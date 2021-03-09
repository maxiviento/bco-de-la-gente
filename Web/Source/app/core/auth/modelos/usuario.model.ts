export class Usuario {

  public nombre: string;
  public apellido: string;
  public cuil: string;
  public email: string;
  public reiniciarToken: boolean

  constructor(nombre?: string,
              apellido?: string,
              cuil?: string,
              email?: string,
              reiniciarToken?: boolean) {

    this.nombre = nombre;
    this.apellido = apellido;
    this.cuil = cuil;
    this.email = email;
    this.reiniciarToken = reiniciarToken;
  }
}
