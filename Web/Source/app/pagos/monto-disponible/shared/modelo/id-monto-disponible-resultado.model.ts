export class IdMontoDisponibleResultado {
  public idMontoDisponible: number;
  public mensaje: string;
  public ok: boolean;

  constructor(idMontoDisponible?: number,
              mensaje?: string,
              ok?: boolean) {
    this.idMontoDisponible = idMontoDisponible;
    this.mensaje = mensaje;
    this.ok = ok;
  }
}
