using System;
using Infraestructura.Core.Comun.Dato;

namespace Soporte.Aplicacion.Consultas.Resultados
{
    public class VigenciaParametroResultado
    {
        public Id Id { get; set; }
        public string Valor { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
    }
}
