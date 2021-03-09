export class BandejaAsignarMontoDisponible {
  public idMontoDisponible: number;
  public nroMontoDisponible: number;
  public fechaAlta: Date;
  public descripcion: string;
  public montoTotal: number;
  public montoUsado: number;
  public montoAUsar: number;
  public seleccionado: boolean;

  constructor(idMontoDisponible?: number,
              nroMontoDisponible?: number,
              fechaAlta?: Date,
              descripcion?: string,
              montoTotal?: number,
              montoUsado?: number,
              montoAUsar?: number,
              seleccionado?: boolean) {
    this.idMontoDisponible = idMontoDisponible;
    this.nroMontoDisponible = nroMontoDisponible;
    this.fechaAlta = fechaAlta;
    this.descripcion = descripcion;
    this.montoTotal = montoTotal;
    this.montoUsado = montoUsado;
    this.montoAUsar = montoAUsar;
    this.seleccionado = seleccionado;
  }
}
