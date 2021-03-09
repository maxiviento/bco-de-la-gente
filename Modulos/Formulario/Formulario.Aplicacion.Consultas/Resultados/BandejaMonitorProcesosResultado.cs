using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class BandejaMonitorProcesosResultado
    {
        public int Id { get; set; }
        public DateTime? FechaAlta { get; set; }
        public DateTime? FechaInicioProceso { get; set; }
        public DateTime? FechaGeneracionPdf { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int? IdEstado { get; set; }
        public int? IdTipo { get; set; }
        public string Estado { get; set; }
        public string Tipo { get; set; }
        public string UsuarioAlta { get; set; }
        public string UsuarioModificacion { get; set; }
        public string PathArchivo { get; set; }
    }
}
