import { FormularioPlanDePagoResultado } from './formulario-plan-de-pago-resultado.model';
export class PlanDePagoGrillaResultado{
  public descripcion: string;
  public formularios: FormularioPlanDePagoResultado[];
  public montoCuota: number;

  constructor(descripcion?: string,
              formularios?: FormularioPlanDePagoResultado[],
              montoCuota?: number) {
    this.descripcion = descripcion;
    this.formularios = formularios;
    this.montoCuota = montoCuota;
  }
}
