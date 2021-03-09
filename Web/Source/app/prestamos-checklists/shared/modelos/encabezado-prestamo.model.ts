export class EncabezadoPrestamo {
  public nroPrestamo: number;
  public nombreLinea: string;
  public nroSticker: string;
  public nombreEstadoPrestamo: string;
  public nombreOrigenPrestamo: string;


  constructor(nroPrestamo?: number,
              nombreLinea?: string,
              nroSticker?: string,
              nombreEstadoPrestamo?: string,
              nombreOrigenPrestamo?: string) {

    this.nroPrestamo = nroPrestamo;
    this.nombreLinea = nombreLinea;
    this.nroSticker = nroSticker;
    this.nombreEstadoPrestamo = nombreEstadoPrestamo;
    this.nombreOrigenPrestamo = nombreOrigenPrestamo;
  }
}
