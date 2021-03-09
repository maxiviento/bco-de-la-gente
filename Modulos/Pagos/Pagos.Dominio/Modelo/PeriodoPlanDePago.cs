using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infraestructura.Core.Comun.Dato;

namespace Pagos.Dominio.Modelo
{
    public class PeriodoPlanDePago: Entidad
    {
        public string Nombre { get; set; }
        public int Dias { get; set; }
    }
}
