using System;

namespace Soporte.Aplicacion.Consultas.Resultados
{
    public class ParametrosGrillaResultado
    {
        public long? IdParametro { get; set; }
        public string NombreParametro { get; set; }
        public string TipoValor { get; set; }
        public string Valor { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public string NombreBeneficio { get; set; }
        public bool? EsEditable { get; set; }
    }
}
