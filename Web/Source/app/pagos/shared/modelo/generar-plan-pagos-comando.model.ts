export class GenerarPlanPagosComando{
  public mesesGracia: number;
  public fechaPago: Date;
  public idsFormularios: number[];


  constructor(mesesGracia?: number,
              fechaPago?: Date,
              idsFormularios?: number[]) {
    this.mesesGracia = mesesGracia;
    this.fechaPago = fechaPago;
    this.idsFormularios = idsFormularios;
  }
}
