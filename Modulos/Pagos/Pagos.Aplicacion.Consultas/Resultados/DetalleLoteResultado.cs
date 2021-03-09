using System;

namespace Pagos.Aplicacion.Consultas.Resultados
{
    public class DetalleLoteResultado
    {
        public decimal? IdLote { get; set; }
        public decimal? Numero { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string FechaCreacionString { get; set; }
        public decimal? MontoPrestamos { get; set; }
        public decimal? Comision { get; set; }
        public decimal? Iva { get; set; }
        public decimal? MontoLote { get; set; }
        public decimal? CantidadPrestamos { get; set; }
        public decimal? CantidadBeneficiarios { get; set; }
        public decimal? NroMonto { get; set; }
        public decimal? IdConvenio { get; set; }
        public string CodigoConvenio { get; set; }
    }
}
