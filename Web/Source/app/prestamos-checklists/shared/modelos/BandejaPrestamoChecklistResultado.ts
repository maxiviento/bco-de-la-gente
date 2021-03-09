export class BandejaPrestamoChecklistResultado {
  public id: number;
  public nroLinea: string;
  public nombreYApellidoSolicitante: string;
  public cuil: string;
  public fechaEnvio: Date;
  public origen: string;
  public estadoPrestamo: string;
  public nroPrestamo: string;
  public nroSticker: string;

  constructor(id?: number,
              nroPrestamo?: string,
              nroSticker?: string,
              nroLinea?: string,
              nombreYApellidoSolicitante?: string,
              cuil?: string,
              fechaEnvio?: Date,
              origen?: string,
              estadoPrestamo?: string) {
    this.id = id;
    this.nroLinea = nroLinea;
    this.nombreYApellidoSolicitante = nombreYApellidoSolicitante;
    this.cuil = cuil;
    this.fechaEnvio = fechaEnvio;
    this.origen = origen;
    this.nroSticker = nroSticker;
    this.nroPrestamo = nroPrestamo;
    this.estadoPrestamo = estadoPrestamo;
  }
}
