using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Excepciones;

namespace Configuracion.Dominio.Modelo
{
    public class TipoItem : Entidad
    {
        public TipoItem()
        {
        }

        public TipoItem(string nombre, string descripcion)
        {
            ValidarIntegridadCampos(nombre, descripcion);
            this.Nombre = nombre;
            this.Descripcion = descripcion;

        }
        private static void ValidarIntegridadCampos(string nombre, string descripcion)
        {
            if (string.IsNullOrEmpty(nombre))
                throw new ModeloNoValidoException("El nombre del tipo de ítem es requerido.");
            if (nombre.Length > 50)
                throw new ModeloNoValidoException("El nombre del el tipo de ítem no puede exceder los 50 caracteres.");
            if (string.IsNullOrEmpty(descripcion))
                throw new ModeloNoValidoException("La descripción del el tipo de ítem es requerido.");
            if (descripcion.Length > 200)
                throw new ModeloNoValidoException("La descripción del el tipo de ítem no puede exceder los 200 caracteres.");
        }

        public virtual string Descripcion { get; protected set; }
        public virtual string Nombre { get; protected set; }
    }
}