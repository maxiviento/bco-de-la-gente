using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class AgrupacionCursosReporteResultado
    {
        public List<SolicitudCursoReporteResultado> CursosConSalidaLaboral { get; set; }
        public List<SolicitudCursoReporteResultado> CursosDeCapacitacion { get; set; }

        public List<SolicitudCursoReporteResultado> CursosConSalidaLaboral2 { get; set; }
        public List<SolicitudCursoReporteResultado> CursosDeCapacitacion2 { get; set; }
        public List<NombreDescripcionOtros> NombreDescripcion { get; set; }
    }
}
