using System;

namespace Configuracion.Dominio.Modelo
{
    public class ParametroTablaDefinidaResult
    {
        public decimal? Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public string NombreMotivoRechazo { get; set; }
        public string Usuario { get; set; }
        public decimal? IdTabla { get; set; }
    }
}