using Infraestructura.Core.Comun.Dato;

namespace Identidad.Dominio.Modelo
{
    public class Url: Entidad
    {
        public virtual string Valor { get; set; }
        public virtual Metodo Metodo { get; set; }
    }
}
