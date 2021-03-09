using System;
using System.Collections.Generic;
using Pagos.Aplicacion.Consultas.Consultas;

namespace Pagos.Aplicacion.Comandos
{
    public class ProvidenciaComando
    {
        public int IdFormulario { get; set; }
        public DateTime Fecha { get; set; }
        public bool FechaManual { get; set; }
        public int IdLote { get; set; }
        public bool FechaAprovacionMasiva { get; set; }
    }
}
