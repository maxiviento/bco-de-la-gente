using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class EstimaClienteReporteResultado : ReporteFormularioResultado
    {
        public bool Estima { get; set; }
        public decimal Cantidad { get; set; }
    }
}
