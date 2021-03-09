export class UsuarioLogueado {

  public nombre: string;
  public apellido: string;
  public cuil: string;
  public email: string;
  public perfilId: number;
  public nombrePerfil: string;

  constructor(nombre?: string,
              apellido?: string,
              cuil?: string,
              email?: string,
              perfilId?: number,
              nombrePerfil?: string) {

    this.nombre = nombre;
    this.apellido = apellido;
    this.cuil = cuil;
    this.email = email;
    this.perfilId = perfilId;
    this.nombrePerfil = nombrePerfil;
  }
}
