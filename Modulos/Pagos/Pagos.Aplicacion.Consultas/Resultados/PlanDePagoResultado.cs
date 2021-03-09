using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pagos.Aplicacion.Consultas.Resultados
{
    public class PlanDePagoResultado
    {
        public int CantCuotas { get; set; }
        public decimal? MontoCuota { get; set; }
        public IList<FormularioPlanDePagoResultado> Formularios { get; set; }

        public string Descripcion
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(CantCuotas)
                    .Append(" cuotas");

                if (MontoCuota != null)
                    sb.Append(" - $ ").Append(MontoCuota);

                return sb.ToString();
            }
        }
    }
}
