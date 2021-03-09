using System;

namespace Pagos.Aplicacion.Consultas.Resultados
{
    public class DatosFechaFormResultado
    {
        public int IdFormulario { get; set; }
        public bool EsAsociativa { get; set; }
        public int EstadoForm { get; set; }
        public DateTime FecInicioPago { get; set; }
        public DateTime FecFinPago { get; set; }
        public decimal? CantMinForms { get; set; }
        public decimal? CantForms { get; set; }
        public string ElementoPago { get; set; }
        public string ModPago { get; set; }
    }
}
