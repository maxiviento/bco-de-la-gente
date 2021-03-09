export class DeudaEmprendimiento {

  public id: number;
  public descripcion: string;
  public monto: number;
  public idTipoDeudaEmprendimiento: number;


  constructor(id?: number,
              descripcion?: string,
              monto?: number,
              IdTipoDeudaEmprendimiento?: number) {

    this.id = id;
    this.descripcion = descripcion;
    this.monto = monto;
    this.idTipoDeudaEmprendimiento = IdTipoDeudaEmprendimiento;
  }
}
