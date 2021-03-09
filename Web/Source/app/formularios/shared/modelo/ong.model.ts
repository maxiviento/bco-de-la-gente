export class OngComboResultado {
  public idEntidad: number;
  public nombre: string;

  constructor(idEntidad?: number,
              nombre?: string) {
    this.idEntidad = idEntidad;
    this.nombre = nombre;
  }
}
