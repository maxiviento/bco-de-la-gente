using System.Collections.Generic;
using Infraestructura.Core.Comun.Dato;

namespace Soporte.Dominio.Modelo
{
    public class RespuestaRentas
    {
        public string status { get; set; }
        public string message { get; set; }
        public string action { get; set; }
        public List<DatoRenta> data { get; set; }
        public string error { get; set; }
    }
}