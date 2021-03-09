using Infraestructura.Core.Comun.Dato;

namespace Formulario.Dominio.Modelo
{
    public class Convenio: Entidad
    {
        public string Nombre { get; set; }
        public decimal? IdTipoConvenio { get; set; }
    }
}