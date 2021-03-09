using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.Modelo;

namespace Formulario.Aplicacion.Comandos
{
    public class OrganizacionEmprendimientoComando
    {
        /// <summary>
        /// Id emprendimento
        /// </summary>
        public int Id { get; set; }
        public int? IdTipoOrganizacion { get; set; }
        public IEnumerable<MiembroEmprendimientoFormularioResultado> Miembros { get; set; }
    }
}
