using System;
using System.Collections.Generic;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class ConsultaDeudaFormularioResultado
    {
        public DateTime? FechaUltimoPago { get; set; }
        public decimal? CantidadCuotasPagas { get; set; }
        public decimal? CantidadCuotasImpagas { get; set; }
        public decimal? CantidadCuotasVencidas { get; set; }
        public int MotivoRechazo { get; set; }
        public string Estado { get; set; }
        public long IdEstadoFormulario { get; set; }
        public DateTime? FechaDefuncion { get; set; }
        public List<int> MotivosRechazo { get; set; }
        public decimal? ImporteCuota { get; set; }
    }
}
