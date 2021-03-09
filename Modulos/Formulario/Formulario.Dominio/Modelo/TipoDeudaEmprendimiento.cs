using Infraestructura.Core.Comun.Dato;

namespace Formulario.Dominio.Modelo
{
    public class TipoDeudaEmprendimiento : Entidad
    {
        public virtual string Nombre { get; set; }
        public virtual string Descripcion { get; set; }

        public TipoDeudaEmprendimiento()
        {
        }

        public TipoDeudaEmprendimiento(string nombre, string descripcion)
        {
            Nombre = nombre;
            Descripcion = descripcion;
        }
    }
}