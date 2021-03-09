using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Core.Comun.Dato
{
    public interface IConsultaConDatosPersona
    {
        long? TipoPersona { get; set; }
        string Nombre { get; set; }
        string Apellido { get; set; }
        string Dni { get; set; }
        string Cuil { get; set; }
        void RevisarInclusionDeFechas();
    }
}
