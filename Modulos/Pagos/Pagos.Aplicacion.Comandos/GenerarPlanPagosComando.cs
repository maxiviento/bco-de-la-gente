using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pagos.Aplicacion.Comandos
{
    public class GenerarPlanPagosComando
    {
        public List<int> IdsFormularios { get; set; }
        public long? IdPrestamo { get; set; }
        public long? IdLote { get; set; }

        public int PeriodoPago { get; set; }
        public int MesesGracia { get; set; }
        public DateTime? FechaPago { get; set; }

        public List<string> Validar()
        {
            List<string> errores = new List<string>();

            if(IdsFormularios?.Count == 0)
                errores.Add("El id del formulario es requerido.");

            //if(PeriodoPago == 0)
            //    errores.Add("El período de pago es requerido.");

            //if(PeriodoPago > 30)
            //    errores.Add("El período máximo de pago es de 30 días.");

            //if(MesesGracia < 0)
            //    errores.Add("Los meses de gracia son requeridos.");

            //if (MesesGracia > 12)
            //    errores.Add("Los meses de gracia máximo son 12 meses.");

            //if (FechaPago == null)
            //    errores.Add("La fecha de pago es requerida");

            if (errores.Count == 0) return null;

            return errores;
        }
    }
}
