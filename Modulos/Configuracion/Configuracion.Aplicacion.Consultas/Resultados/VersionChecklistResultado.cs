using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infraestructura.Core.Comun.Dato;

namespace Configuracion.Aplicacion.Consultas.Resultados
{
    public class VersionChecklistResultado
    {
        public Id Id { get; set; }
        public string Version { get; set; }
        public string EstaEnUso { get; set; }
        public bool EnUso => EstaEnUso == "S";
    }
}
