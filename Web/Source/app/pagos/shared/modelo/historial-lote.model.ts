export class HistorialLote {
  public fechaModificacionLote: Date;
  public nombre: string;
  public cantPrestamos: number;
  public cantBeneficiarios: number;
  public montoTotalPrestamo: number;
  public montoComision: number;
  public montoIva: number;
  public montoTotalLote: number;
  public usuarioModificacion: string;

  constructor(fechaModificacionLote?: Date,
              nombre?: string,
              cantPrestamos?: number,
              cantBeneficiarios?: number,
              montoTotalPrestamo?: number,
              montoComision?: number,
              montoIva?: number,
              montoTotalLote?: number,
              usuarioModificacion?: string) {
    this.fechaModificacionLote = fechaModificacionLote;
    this.nombre = nombre;
    this.cantPrestamos = cantPrestamos;
    this.cantBeneficiarios = cantBeneficiarios;
    this.montoTotalPrestamo = montoTotalPrestamo;
    this.montoComision = montoComision;
    this.montoIva = montoIva;
    this.montoTotalLote = montoTotalLote;
    this.usuarioModificacion = usuarioModificacion;
  }
}
