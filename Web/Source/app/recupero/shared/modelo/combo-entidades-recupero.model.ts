export class ComboEntidadesRecupero {
  public id: number;
  public nombre: string;
  public abreviatura: string;

  constructor(id?: number,
              nombre?: string,
              abreviatura?: string) {
    this.id = id;
    this.nombre = nombre;
    this.abreviatura = abreviatura;
  }
}
