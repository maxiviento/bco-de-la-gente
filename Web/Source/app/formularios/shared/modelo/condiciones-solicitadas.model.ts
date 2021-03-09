// Condiciones del prestamo cargadas para algun formulario
export class CondicionesSolicitadas {
  public id: number;
  public descripcion: string;
  public montoSolicitado: number;
  public cantidadCuotas: number;
  public montoEstimadoCuota: number;

  public constructor(id?: number,
                     descripcion?: string,
                     montoSolicitado?: number,
                     cantidadCuotas?: number,
                     montoEstimadoCuota?: number) {
    this.id = id;
    this.descripcion = descripcion;
    this.montoSolicitado = montoSolicitado;
    this.cantidadCuotas = cantidadCuotas;
    this.montoEstimadoCuota = montoEstimadoCuota;
  }
}
