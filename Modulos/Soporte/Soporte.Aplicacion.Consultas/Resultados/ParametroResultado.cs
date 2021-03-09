using System.Collections.Generic;
using Infraestructura.Core.Comun.Dato;

namespace Soporte.Aplicacion.Consultas.Resultados
{
    public class ParametroResultado
    {
        public Id Id { get; set; }
        public IList<VigenciaParametroResultado> VigenciasParametro { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}
