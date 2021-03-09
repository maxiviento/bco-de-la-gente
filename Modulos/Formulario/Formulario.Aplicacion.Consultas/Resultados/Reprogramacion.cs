using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class Reprogramacion
    {
        public decimal? Id { get; set; }
        public DateTime FechaInicioPago { get; set; }
        public DateTime FechaFinPago { get; set; }
        public DateTime FechaModif { get; set; }
        public string NombreUsuario { get; set; }
    }
}
