using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class MotivosRechazoFormularioResultado
    {
        public decimal? IdMotivo { get; set; }
        public DateTime FechaRechazo { get; set; }
        public string NombreMotivo { get; set; }
        public string Observaciones { get; set; }
        public decimal? IdSeguimientoFormulario { get; set; }
        public string Descripcion { get; set; }
        public string Abreviatura { get; set; }
    }
}
