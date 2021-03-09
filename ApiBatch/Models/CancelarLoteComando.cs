using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;

namespace ApiBatch.Infraestructure
{
    public class CancelarLoteComando
    {
        public virtual Id Id { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Id EstadoAnteriorLoteId { get; set; }
        public virtual string Error { get; set; }
        public virtual string Mensaje { get; set; }
    }
}