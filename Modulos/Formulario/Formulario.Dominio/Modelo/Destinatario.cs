using Infraestructura.Core.Comun.Dato;

namespace Formulario.Dominio.Modelo
{
    public class Destinatario: Entidad
    {
        protected Destinatario()
        {
        }

        public Destinatario(string descripcion)
        {
            Descripcion = descripcion;
        }

        public virtual string Descripcion { get; protected set; }
    }
}
