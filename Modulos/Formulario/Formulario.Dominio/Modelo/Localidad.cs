using Infraestructura.Core.Comun.Dato;

namespace Formulario.Dominio.Modelo
{
    public class Localidad: Entidad
    {
        protected Localidad()
        {
        }

        public Localidad(Id id, string descripcion, Id idDepartamento)
        {
            Id = id;
            Descripcion = descripcion;
            IdDepartamento = idDepartamento;
        }
        
        public virtual string Descripcion { get; protected set; }
        public virtual Id IdDepartamento { get; protected set; }
    }
}
