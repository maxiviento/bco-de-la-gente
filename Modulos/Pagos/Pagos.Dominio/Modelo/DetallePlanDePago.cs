using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infraestructura.Core.Comun.Dato;

namespace Pagos.Dominio.Modelo
{
    public class DetallePlanDePago: Entidad
    {
        public DetallePlanDePago() { }

        public DetallePlanDePago(int numCuota, DateTime fechaCuota, double montoCuota)
        {
            FechaCuota = fechaCuota;
            MontoCuota = montoCuota;
            NumCuota = numCuota;
        }

        public DateTime FechaCuota { get; set; }
        public double MontoCuota { get; set; }
        public int NumCuota { get; set; }
    }
}
