using Infraestructura.Core.Comun.Dato;

namespace Formulario.Dominio.Modelo
{
    public class TipoInversion: Entidad
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public static TipoInversion InversionRealizada => new TipoInversion { Id = new Id(1), Nombre = "INVERSION REALIZADA"};
        public static TipoInversion NecesidadInversion => new TipoInversion { Id = new Id(2), Nombre = "NECESIDAD INVERSION" };
    }
}
