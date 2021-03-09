using System;
using Infraestructura.Core.Comun.Presentacion;

namespace Pagos.Aplicacion.Consultas.Consultas
{
    public class BandejaRecuperoConsulta : Consulta
    {
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public int? IdTipoEntidad { get; set; }
    }
}
