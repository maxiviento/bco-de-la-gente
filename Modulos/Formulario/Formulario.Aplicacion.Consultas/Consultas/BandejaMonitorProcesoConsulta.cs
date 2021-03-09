using Infraestructura.Core.Comun.Presentacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulario.Aplicacion.Consultas.Consultas
{
    public class BandejaMonitorProcesoConsulta : Consulta
    {
        public DateTime FechaAlta { get; set; }

        public string IdsEstado { get; set; }

        public string IdsTipo { get; set; }

        public int? IdUsuario { get; set; }

    }
}
