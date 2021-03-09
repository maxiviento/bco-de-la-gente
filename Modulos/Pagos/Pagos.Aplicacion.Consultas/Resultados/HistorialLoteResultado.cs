using System;

namespace Pagos.Aplicacion.Consultas.Resultados
{
    public class HistorialLoteResultado
    {
        public DateTime FechaModificacionLote { get; set; }
        public string Nombre { get; set; }
        public decimal? CantPrestamos { get; set; }
        public decimal? CantBeneficiarios { get; set; }
        public decimal? MontoTotalPrestamo { get; set; }
        public decimal? MontoComision { get; set; }
        public decimal? MontoIva { get; set; }
        public decimal? MontoTotalLote { get; set; }
        public string UsuarioModificacion { get; set; }
    }
}
