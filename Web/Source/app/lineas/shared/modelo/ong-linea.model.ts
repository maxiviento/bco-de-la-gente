export class OngLinea {
  public id: number;
  public idLineaOng: number;
  public nombre: string;
  public porcentajePago: string;
  public porcentajeRecupero: string;

  public constructor(
    id?: number,
    idLineaOng?: number,
    nombre?: string,
    porcentajePago?: string,
    porcentajeRecupero?: string
  ) {
    this.id = id;
    this.idLineaOng = idLineaOng;
    this.nombre = nombre;
    this.porcentajePago = porcentajePago;
    this.porcentajeRecupero = porcentajeRecupero;
  }
}
