export class DetallePlanDePago{
  public nroCuota: number;
  public montoCuota: number;
  public fechaCuota: Date;
  public fechaPago: Date;
  public extraAlPlan: boolean;
  public montoCuotaReal: number;

  constructor(nroCuota?: number,
              montoCuota?: number,
              fechaCuota?: Date,
              fechaPago?: Date,
              extraAlPlan?: boolean,
              montoCuotaReal?: number) {
    this.nroCuota = nroCuota;
    this.montoCuota = montoCuota;
    this.fechaCuota = fechaCuota;
    this.fechaPago = fechaPago;
    this.extraAlPlan = extraAlPlan;
    this.montoCuotaReal = montoCuotaReal;
  }
}
