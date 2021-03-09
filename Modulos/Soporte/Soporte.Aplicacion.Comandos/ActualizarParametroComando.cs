using System;

namespace Soporte.Aplicacion.Comandos
{
    public class ActualizarParametroComando
    {
        public virtual long? Id { get; set; }
        public virtual long? IdVigencia { get; set; }
        public string Valor { get; set; }
        public DateTime? FechaDesde { get; set; }
    }
}
