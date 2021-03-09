using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pagos.Aplicacion.Consultas.Resultados
{
    public class FormularioPlanDePagoResultado
    {
        public long NroFormulario { get; set; }
        public int CantCuotas { get; set; }
        public IList<DetallePlanDePagoGrillaResultado> Detalles { get; set; }

        public decimal MontoTotal
        {
            get
            {
                decimal montoTotal = 0;
                if (Detalles != null && Detalles.Count > 0)
                {
                    int i = 0;
                    foreach (var detalle in Detalles)
                    {
                        montoTotal += detalle.MontoCuota.GetValueOrDefault();
                    }
                }
                return montoTotal;
            }
        }

        public decimal MontoPagado
        {
            get
            {
                decimal res = 0;
                if (Detalles != null && Detalles.Count > 0)
                {
                    foreach (var detalle in Detalles)
                    {
                        res += detalle.MontoCuotaReal ?? 0;
                    }
                }
                return res;
            }
        }
    }
}
