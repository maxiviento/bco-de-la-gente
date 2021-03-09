using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infraestructura.Core.Comun.Presentacion;

namespace Configuracion.Dominio.Modelo
{
    public class ConsultaTablasDefinidas: Consulta
    {
        public decimal? IdTabla { get; set; }
        public string Nombre { get; set; }
    }
}
