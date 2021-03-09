export class MovimientosMontoDisponible {
  public id: number;
  public descripcion: string;
  public tipoUso: string;
  public fechaUso: Date;
  public valorMovimiento: number;
  public saldo: number;
  public usuario: string;

  constructor(id?: number,
              descripcion?: string,
              tipoUso?: string,
              fechaUso?: Date,
              datosUso?: string,
              valorMovimiento?: number,
              saldo?: number,
              usuario?: string) {
    this.id = id;
    this.descripcion = descripcion;
    this.tipoUso = tipoUso;
    this.fechaUso = fechaUso;
    this.valorMovimiento = valorMovimiento;
    this.saldo = saldo;
    this.usuario = usuario;
  }
}
