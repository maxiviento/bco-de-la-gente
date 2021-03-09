using Infraestructura.Core.Comun.Dato;

namespace Formulario.Dominio.Modelo
{
    public class Departamento : Entidad
    {
        public virtual string Descripcion { get; protected set; }

        protected Departamento()
        {
        }

        public Departamento(Id id, string descripcion)
        {
            Id = id;
            Descripcion = descripcion;
        }
    }
}