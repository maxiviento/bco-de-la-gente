using System;
using Infraestructura.Core.Comun.Presentacion;

namespace Pagos.Aplicacion.Consultas.Consultas
{
    public class BandejaSuafConsulta : Consulta
    {
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public decimal IdLote { get; set; }
    }
}
