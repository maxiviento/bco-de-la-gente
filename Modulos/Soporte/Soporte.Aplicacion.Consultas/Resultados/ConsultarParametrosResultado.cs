using System;
using Infraestructura.Core.Comun.Dato;

namespace Soporte.Aplicacion.Consultas.Resultados
{
    public class ConsultarParametrosResultado
    {
        public Id Id { get; set; }
        public Id? IdVigencia { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public long IdTipoDato { get; set; }
        public string Valor { get; set; }
        public string NombreTipoDato { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public bool Vigente { get; set; }
    }
}