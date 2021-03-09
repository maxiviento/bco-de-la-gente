using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Excepciones;

namespace Formulario.Dominio.Modelo
{
    public class Cuadrante : Entidad
    {
        public virtual string Nombre { get; protected set; }
        public virtual string Descripcion { get; protected set; }

        public Cuadrante()
        {
        }

        public Cuadrante(Id id) : this(id, null, null)
        {
        }

        public Cuadrante(Id id, string nombre, string descripcion)
        {
            if (id != null)
                Id = id;
            if (!string.IsNullOrEmpty(nombre) && nombre.Length > 100)
                throw new ModeloNoValidoException("El nombre del cuadrante no puede superar los 100 caracteres");
            if (!string.IsNullOrEmpty(descripcion) && descripcion.Length > 200)
                throw new ModeloNoValidoException("El nombre del cuadrante no puede superar los 200 caracteres");
        }
    }
}

