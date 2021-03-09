using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;

namespace Identidad.Aplicacion.Consultas.Consultas
{
    public class UsuarioConFiltrosConsulta : Consulta
    {
        public string Cuil { get; set; }
        public Id PerfilId { get; set; }
        public bool IncluyeBajas { get; set; } = false;
    }
}
