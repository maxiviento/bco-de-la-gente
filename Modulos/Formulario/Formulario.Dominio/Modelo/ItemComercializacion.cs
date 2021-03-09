using Infraestructura.Core.Comun.Dato;

namespace Formulario.Dominio.Modelo
{
    public class ItemComercializacion: Entidad
    {
        public ItemComercializacion() { }
        public ItemComercializacion(int id)
        {
            Id = new Id(id);
        }
        public ItemComercializacion(int id, string nombre, string descripcion)
        {
            Id = new Id(id);
            Nombre = nombre;
            Descripcion = descripcion;
        }

        public string Nombre { get; protected set; }
        public string Descripcion { get; protected set; }
        public virtual TipoCurso TipoItemComercializacion { get; protected set; }
    }
}
