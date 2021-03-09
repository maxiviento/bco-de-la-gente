using System.Collections.Generic;
using Infraestructura.Core.Comun.Dato;

namespace Identidad.Dominio.Modelo
{
    public class Funcionalidad : Entidad
    {
        public virtual string Nombre { get; set; }
        public virtual string Codigo { get; set; }
        public virtual IList<Url> Urls { get; set; }
    }
}
