using System;
using Infraestructura.Core.Comun.Presentacion;

namespace Soporte.Aplicacion.Consultas
{
    public class ParametrosConsultaFiltros : Consulta
    {
        public ParametrosConsultaFiltros()
        {
            IdParametro = -1;
            FechaDesde = new DateTime(0001, 01, 01);
            FechaHasta = new DateTime(0001, 01, 01);
            SoloVigentes = "S";
        }

        public long IdParametro { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public string SoloVigentes { get; set; }
    }
}