using System;
using System.Collections.Generic;

namespace Pagos.Aplicacion.Consultas.Consultas
{
    public class ReportePagosConsulta
    {
        public int? IdLote { get; set; }
        public int? IdPrestamo { get; set; }
        public int? IdFormularioLinea { get; set; }
        public int IdOpcion { get; set; }
        public List<int> IdsFormularios { get; set; }
        public List<int> IdsReportesPagos { get; set; }
        public bool FechaAprobacion { get; set; }
        public DateTime Fecha { get; set; }
    }
}
