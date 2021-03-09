using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class GrupoFamiliarDomicilioResultado
    {
        public Id Id { get; set; }
        public Id IdGrupoUnico { get; set; }
        public List<SituacionDomicilioIntegranteResultado> ListadoPersonas { get; set; }
    }
}
