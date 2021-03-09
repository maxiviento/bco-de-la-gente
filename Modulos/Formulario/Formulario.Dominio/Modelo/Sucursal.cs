using Infraestructura.Core.Comun.Dato;

namespace Formulario.Dominio.Modelo
{
    public class Sucursal 
    {
        protected Sucursal()
        {
        }

        public Sucursal(string id, string descripcion, Id idDepartamento)
        {
            Id = id;
            Descripcion = descripcion;
            IdDepartamento = idDepartamento;
        }
        public Sucursal(string id, string nombre)
        {
            Id = id;
            Nombre = nombre;
        }

        public virtual string Id { get; protected set; }
        public virtual string Descripcion { get; protected set; }
        public virtual string Nombre { get; protected set; }
        public virtual Id IdDepartamento { get; protected set; }
    }
}
