using System;

namespace Pagos.Aplicacion.Consultas.Resultados
{
    public class DatosNotaPago
    {
        public string FechaNota { get; set; }
        public string Nombre { get; set; }
        public string Cc { get; set; }
        public string CuentaCorriente { get; set; }
        public decimal? MontoNumero { get; set; }
        public string MontoLetra { get; set; }
        public string Empresa { get; set; }
        public decimal? CantRegistros { get; set; }
        public DateTime FechaInicioPago { get; set; }
        public DateTime FechaFinPago { get; set; }
    }
}
