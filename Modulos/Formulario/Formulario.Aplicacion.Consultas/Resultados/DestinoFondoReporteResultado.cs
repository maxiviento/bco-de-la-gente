using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class DestinoFondoReporteResultado: ReporteFormularioResultado
    {
        public DestinoFondoReporteResultado() { }

        public DestinoFondoReporteResultado(string nombre, string descripcion, string observaciones, bool seleccionado)
        {
            Observaciones = observaciones;
            Nombre = nombre;
            Descripcion = descripcion;
            Seleccionado = seleccionado;
        }

        public string Observaciones { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Seleccionado { get; set; }
    }
}
