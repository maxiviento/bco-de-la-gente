using Infraestructura.Core.Comun.Dato;

namespace Formulario.Dominio.Modelo
{
    public class TipoProyecto : Entidad
    {
        public TipoProyecto() { }
        public TipoProyecto(int id)
        {
            Id = new Id(id);
        }
        public TipoProyecto(int id, string nombre, string descripcion)
        {
            Id = new Id(id);
            Nombre = nombre;
            Descripcion = descripcion;
        }
        public virtual string Nombre { get; protected set; }
        public virtual string Descripcion { get; protected set; }
    }
}