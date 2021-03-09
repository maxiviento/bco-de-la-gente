using Infraestructura.Core.Comun.Presentacion;

namespace Identidad.Aplicacion.Consultas.Consultas
{
    public class PerfilConFiltrosConsulta : Consulta
    {
        public string Nombre { get; set; }
        public bool IncluirBajas { get; set; } = false;
    }
}
