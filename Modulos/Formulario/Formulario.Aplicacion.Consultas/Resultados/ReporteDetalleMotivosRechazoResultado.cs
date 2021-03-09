using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class ReporteDetalleMotivosRechazoResultado
    {
        public ReporteDetalleMotivosRechazoResultado() { }

        public ReporteDetalleMotivosRechazoResultado(int id, string nombre, string descripcion, string abreviatura)
        {
            Id = id;
            Nombre = nombre;
            Descripcion = descripcion;
            Abreviatura = abreviatura;
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Abreviatura { get; set; }
    }
}
