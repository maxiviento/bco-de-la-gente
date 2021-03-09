export class ReactivarPrestamoComando {
  public idFormulario: number;
  public idPrestamo: number;
  public observacion: string;

  constructor(idFormulario?: number,
              idPrestamo?: number,
              observacion?: string) {
    this.idFormulario = idFormulario;
    this.idPrestamo = idPrestamo;
    this.observacion = observacion;
  }
}
