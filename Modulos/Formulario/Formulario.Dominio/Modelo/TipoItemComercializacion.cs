using Infraestructura.Core.Comun.Dato;

namespace Formulario.Dominio.Modelo
{
    public class TipoItemComercializacion : Entidad
    {
        protected TipoItemComercializacion()
        {
        }

        public virtual string Nombre { get; protected set; }
        public virtual string Descripcion { get; protected set; }
    }
}
