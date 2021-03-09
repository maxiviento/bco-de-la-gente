using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Excepciones;

namespace Configuracion.Dominio.Modelo
{
    public class Recurso : Entidad
    {
        public virtual string Nombre { get; protected set; }
        public virtual string Descripcion { get; protected set; }
        public virtual string Url { get; protected set; }

        public Recurso()
        {
        }

        public Recurso(Id IdRecurso) : this(IdRecurso, null, null, null) { }

        public Recurso(Id idRecurso, string nombre, string descripcion, string url)
        {
            if (!string.IsNullOrEmpty(nombre) && nombre.Length > 100)
                throw new ModeloNoValidoException("El nombre del recurso no puede exceder los 100 caracteres.");
            if (!string.IsNullOrEmpty(descripcion) && descripcion.Length > 200)
                throw new ModeloNoValidoException("La descripción del recurso no puede exceder los 200 caracteres.");
            if (url.Length > 2048)
                throw new ModeloNoValidoException("La url del recurso no puede exceder los 2048 caracteres.");
            Id = idRecurso;
            Nombre = nombre;
            Descripcion = descripcion;
            Url = url;
        }
    }
}