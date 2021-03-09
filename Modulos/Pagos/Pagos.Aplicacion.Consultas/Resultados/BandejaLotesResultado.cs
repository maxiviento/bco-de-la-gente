using System;

namespace Pagos.Aplicacion.Consultas.Resultados
{
    public class BandejaLotesResultado
    {
        public decimal? IdLote { get; set; }
        public DateTime FechaLote { get; set; }
        public decimal? NroLote { get; set; }
        public string NombreLote { get; set; }
        public decimal? CantPrestamos { get; set; }
        public decimal? CantBeneficiarios { get; set; }
        public decimal? MontoTotal { get; set; }
        public decimal? Comision { get; set; }
        public decimal? Iva { get; set; }
        public decimal? IdTipoLote { get; set; }
        public bool PermiteLiberar { get; set; }
    }
}
