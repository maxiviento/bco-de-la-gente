using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pagos.Aplicacion.Consultas.Resultados
{
    public class DetallePlanDePagoGrillaResultado
    {
        public DateTime FechaCuota { get; set; }
        public decimal? MontoCuota { get; set; }
        public decimal? MontoCuotaReal { get; set; }
        public int NroCuota { get; set; }
        public int CantCuotas { get; set; }
        public DateTime? FechaPago { get; set; }
        public string Extra { get; set; }
        public bool ExtraAlPlan => !string.IsNullOrEmpty(Extra) && Extra == "S";
        public long NroFormulario { get; set; }

        public bool EsIgualA(DetallePlanDePagoGrillaResultado detalle)
        {
            if (CantCuotas != detalle.CantCuotas) return false;
            if (MontoCuota != detalle.MontoCuota) return false;
            if (NroCuota != detalle.NroCuota) return false;
            return true;
        }
    }
}
