using Infraestructura.Core.Comun.Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class OngLineaResultado
    {
        public Id IdOng { get; set; }
        public decimal? IdLineaOng { get; set; }
        public string NombreOng { get; set; }
        public decimal? PorcentajeRecupero { get; set; }
        public decimal? PorcentajePrestamo { get; set; }
    }
}
