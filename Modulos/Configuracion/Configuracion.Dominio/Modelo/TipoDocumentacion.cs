using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Excepciones;

namespace Configuracion.Dominio.Modelo
{
    public class TipoDocumentacion : Entidad
    {
        public TipoDocumentacion()
        {
        }

        public TipoDocumentacion(string descripcion)
        {
            if (!string.IsNullOrEmpty(descripcion) && descripcion.Length > 200)
                throw new ModeloNoValidoException("La descripción no puede superar los 200 caracteres");
            Descripcion = descripcion;
        }

        public virtual string Descripcion { get; set; }
    }
}