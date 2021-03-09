using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class NombreDescripcionOtros
    {
        public NombreDescripcionOtros() { }

        public NombreDescripcionOtros(string descripcionSalidaLaboral, string descripcionCapacitacion)
        {
            DescripcionOtrosCursosSalidaLaboral = descripcionSalidaLaboral;
            DescripcionOtrosCursosCapacitacion = descripcionCapacitacion;
        }

        public string DescripcionOtrosCursosSalidaLaboral { get; set; }
        public string DescripcionOtrosCursosCapacitacion { get; set; }
    }
}
