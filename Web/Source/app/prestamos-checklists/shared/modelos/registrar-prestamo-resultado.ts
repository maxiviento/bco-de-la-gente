export class RegistrarPrestamoResultado {
  public idPrestamo: number;
  public numeroPrestamo: number;
  public apellidoNombre: string;
  public cuil: string;

  constructor(idPrestamo?: number,
              numeroPrestamo?: number,
              apellidoNombre?: string,
              cuil?: string) {
    this.idPrestamo = idPrestamo;
    this.numeroPrestamo = numeroPrestamo;
    this.apellidoNombre = apellidoNombre;
    this.cuil = cuil;
  }
}
