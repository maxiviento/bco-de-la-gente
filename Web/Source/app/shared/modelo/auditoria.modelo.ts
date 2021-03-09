export class Auditoria {

  public id: number;
  public idFormularioLinea: number;
  public idSeguimientoFormulario: number;
  public idAccion: number;
  public idPrestamoItem: number;
  public idUsuario: number;
  public observaciones: string;

  constructor(id?: number,
              idFormularioLinea?: number,
              idSeguimientoFormulario?: number,
              idAccion?: number,
              idPrestamoItem?: number,
              idUsuario?: number,
              observaciones?: string) {

    this.id = id;
    this.idFormularioLinea = idFormularioLinea;
    this.idSeguimientoFormulario = idSeguimientoFormulario;
    this.idAccion = idAccion;
    this.idPrestamoItem = idPrestamoItem;
    this.idUsuario = idUsuario;
    this.observaciones = observaciones;
  }
}
