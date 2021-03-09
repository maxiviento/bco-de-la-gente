using System;

namespace Pagos.Aplicacion.Consultas.Resultados
{
    public class FormularioPrestamoResultado
    {
        public string Beneficiario { get; set; }
        public string Cuil { get; set; }
        public string Origen { get; set; }
        public string TipoDocumento { get; set; }
        public string NroDocumento { get; set; }
        public string NroFormulario { get; set; }
        public decimal? MontoFormulario { get; set; }
        public int IdFormulario { get; set; }
        public DateTime FechaPago { get; set; }
        public DateTime FechaFinPago { get; set; }
        public int IdEstado { get; set; }
        public String Estado { get; set; }
        public int TipoApoderado { get; set; }
    }
}
