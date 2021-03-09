using Infraestructura.Core.Comun.Dato;

namespace Configuracion.Aplicacion.Consultas.Resultados
{
    public class ConsultarRecursoResultado
    {
        public Id Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Url { get; set; }
    }
}