import { MotivoRechazoAbreviatura } from '../../../motivos-rechazo/shared/modelo/motivos-rechazo-abreviatura.model';

export class ConsultaFormulariosSituacion {
  constructor ( public numeroLinea: number,
                public origen: number,
                public numeroFormulario: number,
                public estadoFormulario: string,
                public motivoRechazoFormulario: MotivoRechazoAbreviatura,
                public numeroPrestamo: number,
                public estadoPrestamo: number,
                public motivoRechazoPrestamo: MotivoRechazoAbreviatura,
                public importe: number,
                public cantidadCuotas: number,
                public cantidadCuotasPagas: number) {
  }
}
