using Infraestructura.Core.Comun.Dato;

namespace Formulario.Dominio.Modelo
{
    public class TipoProceso : Entidad
    {
        public TipoProceso()
        {
        }

        public TipoProceso(string nombre, string descripcion, string abreviatura)
        {
            Nombre = nombre;
            Descripcion = descripcion;
        }

        public virtual string Nombre { get; set; }
        public virtual string Descripcion { get; set; }
        public virtual string Abreviatura { get; set; }
    }
}
