export class BandejaRecuperoResultado {
  public idCabecera: number;
  public fechaRecepcion: Date;
  public entidad: string;
  public nombreArchivo: string;
  public cantTotal: number;
  public cantProc: number;
  public cantEspec: number;
  public cantIncons: number;

  constructor(idCabecera?: number,
              fechaRecepcion?: Date,
              entidad?: string,
              nombreArchivo?: string,
              cantTotal?: number,
              cantProc?: number,
              cantEspec?: number,
              cantIncons?: number) {
    this.idCabecera = idCabecera;
    this.fechaRecepcion = fechaRecepcion;
    this.entidad = entidad;
    this.nombreArchivo = nombreArchivo;
    this.cantTotal = cantTotal;
    this.cantProc = cantProc;
    this.cantEspec = cantEspec;
    this.cantIncons = cantIncons;
  }
}
