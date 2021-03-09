using Infraestructura.Core.Comun.Dato;

namespace Pagos.Aplicacion.Consultas.Resultados
{
    public class Convenio
    {
        public Id Id { get; set; }
        public int IdTipoConvenio { get; set; }
        public string Nombre { get; set; }
    }
}
