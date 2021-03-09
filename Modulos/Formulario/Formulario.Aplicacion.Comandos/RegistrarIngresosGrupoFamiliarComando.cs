using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Formulario.Aplicacion.Consultas.Resultados;

namespace Formulario.Aplicacion.Comandos
{
    public class RegistrarIngresosGrupoFamiliarComando
    {
        public int IdFormulario { get; set; }
        public IList<IngresoGrupoResultado> IngresosGrupo { get; set; }
    }
}
