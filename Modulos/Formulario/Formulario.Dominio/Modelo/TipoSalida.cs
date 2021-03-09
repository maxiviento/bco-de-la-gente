using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Excepciones;

namespace Formulario.Dominio.Modelo
{
    public class TipoSalida : Entidad
    {
        public virtual string Nombre { get; protected set; }
        public virtual string Descripcion { get; protected set; }

        public TipoSalida()
        {
        }

        public TipoSalida(Id id) : this(id, null, null)
        {
        }

        public TipoSalida(Id id, string nombre, string descripcion)
        {
            if (id != null)
                Id = id;
            if (!string.IsNullOrEmpty(nombre) && nombre.Length > 100)
                throw new ModeloNoValidoException("El nombre del tipo de salida no puede superar los 100 caracteres");
            if (!string.IsNullOrEmpty(descripcion) && descripcion.Length > 200)
                throw new ModeloNoValidoException(
                    "La descripcion del tipo de salida no puede superar los 200 caracteres");
        }
    }
}