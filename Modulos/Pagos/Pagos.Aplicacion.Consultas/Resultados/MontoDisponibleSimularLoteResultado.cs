using System;

namespace Pagos.Aplicacion.Consultas.Resultados
{
    public class MontoDisponibleSimularLoteResultado
    {
        public decimal? IdMontoDisponible { get; set; }
        public decimal? NroMontoDisponible { get; set; }
        public DateTime FechaAlta { get; set; }
        public string Descripcion { get; set; }
        public decimal? MontoTotal { get; set; }
        public decimal? MontoUsado { get; set; }
    }
}
