using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class ValidacionEdadResultado
    {
        public ValidacionEdadResultado() { }

        public ValidacionEdadResultado(string descripcion)
        {
            Descripcion = descripcion;
        }

        public string Descripcion { get; set; }
    }
}
