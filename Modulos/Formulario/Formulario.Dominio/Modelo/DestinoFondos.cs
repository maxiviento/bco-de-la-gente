using Infraestructura.Core.Comun.Dato;

namespace Formulario.Dominio.Modelo
{
    public class DestinoFondos : Entidad
    {
        protected DestinoFondos()
        {
        }

        public DestinoFondos(string nombre, string descripcion)
        {
            Nombre = nombre;
            Descripcion = descripcion;
        }

        public string Nombre { get; protected set; }
        public string Descripcion { get; protected set; }
    }
}