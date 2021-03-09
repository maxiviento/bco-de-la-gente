using Infraestructura.Core.Comun.Dato;

namespace Formulario.Dominio.Modelo
{
    public class TipoIntegranteSocio: Entidad
    {
        public TipoIntegranteSocio()
        {
        }

        public TipoIntegranteSocio(string descripcion)
        {
            Descripcion = descripcion;
        }

        public virtual string Descripcion { get; set; }
        public virtual string Nombre { get; set; }
    }
}
