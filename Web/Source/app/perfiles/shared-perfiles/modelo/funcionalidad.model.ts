export class Funcionalidad {
  public seleccionado: boolean;
  public id: string;
  public nombre: string;
  public url: string;
  public codigo: string;

  constructor(clave?: string,
              valor?: string,
              url?: string,
              seleccionado?: boolean,
              codigo?: string) {
    this.seleccionado = seleccionado;
    this.id = clave;
    this.nombre = valor;
    this.url = url;
    this.codigo = codigo;
  }
}
