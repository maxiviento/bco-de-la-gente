export class Reprogramacion {
  public id: number;
  public fechaInicioPago: Date;
  public fechaFinPago: Date;
  public fechaModif: Date;
  public nombreUsuario: string;

  constructor(id?: number,
              fechaInicioPago?: Date,
              fechaFinPago?: Date,
              fechaModif?: Date,
              nombreUsuario?: string) {
    this.id = id;
    this.fechaInicioPago = fechaInicioPago;
    this.fechaFinPago = fechaFinPago;
    this.fechaModif = fechaModif;
    this.nombreUsuario = nombreUsuario;
  }
}
