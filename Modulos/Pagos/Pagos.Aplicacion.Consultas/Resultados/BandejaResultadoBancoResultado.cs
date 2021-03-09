using System;

namespace Pagos.Aplicacion.Consultas.Resultados
{
    public class BandejaResultadoBancoResultado
    {
        public decimal? IdCabecera { get; set; }
        public DateTime FechaRecepcion { get; set; }
        public decimal? Importe { get; set; }
        public string Periodo { get; set; }
        public string FormaPago { get; set; }
        public string TipoPago { get; set; }
        public string Banco { get; set; }
    }
}
