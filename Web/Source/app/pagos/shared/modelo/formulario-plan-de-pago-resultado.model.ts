import { DetallePlanDePago } from './detalle-plan-pago.model';

export class FormularioPlanDePagoResultado{
  public nroFormulario: number;
  public detalles: DetallePlanDePago[];
  public montoTotal: number;
  public montoPagado: number;

  constructor(nroFormulario?: number,
              detalles?: DetallePlanDePago[],
              montoTotal?: number,
              montoPagado?: number) {
    this.nroFormulario = nroFormulario;
    this.detalles = detalles;
    this.montoTotal = montoTotal;
    this.montoPagado = montoPagado;
  }
}
