using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infraestructura.Core.Comun.Dato;

namespace Pagos.Dominio.Modelo
{
    public class PlanDePago: Entidad
    {
        public int MesesDeGracia { get; set; }
        public PeriodoPlanDePago Periodo { get; set; }
        public List<DetallePlanDePago> DetallePlanDePago { get; set; }
    }
}
