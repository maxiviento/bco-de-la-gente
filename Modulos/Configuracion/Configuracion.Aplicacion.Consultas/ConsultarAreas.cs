using Infraestructura.Core.Comun.Presentacion;

namespace Configuracion.Aplicacion.Consultas
{
    public class ConsultarAreas: Consulta
    {
        public string Nombre { get; set; }
        public bool IncluirDadosDeBaja { get; set; } = false;
    }
}