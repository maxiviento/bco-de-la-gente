export class FiltrosPerfiles {
  public nombre: string;
  public incluirBajas: string;
  public numeroPagina: number;

  constructor(nombre?: string,
              incluirBajas?: string,
              numeroPagina?: number) {
    this.nombre = nombre;
    this.incluirBajas = incluirBajas;
    this.numeroPagina = numeroPagina;
  }
}
