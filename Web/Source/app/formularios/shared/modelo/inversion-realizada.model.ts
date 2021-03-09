export class InversionRealizada {
  public id: number;
  public idTipoInversion: number;
  public idItemInversion: number;
  public idInversionEmprendimiento: number;
  public observaciones: string;
  public cantidadNuevos: number;
  public cantidadUsados: number;
  public precioNuevos: number;
  public precioUsados: number;

  constructor(id?: number,
              idTipoInversion?: number,
              idItemInversion?: number,
              idInversionEmprendimiento?: number,
              observaciones?: string,
              cantidadNuevos?: number,
              cantidadUsados?: number,
              precioNuevos?: number,
              precioUsados?: number) {

    this.id = id;
    this.idTipoInversion = idTipoInversion;
    this.idItemInversion = idItemInversion;
    this.idInversionEmprendimiento = idInversionEmprendimiento;
    this.observaciones = observaciones;
    this.cantidadNuevos = cantidadNuevos;
    this.cantidadUsados = cantidadUsados;
    this.precioNuevos = precioNuevos;
    this.precioUsados = precioUsados;
  }
}
