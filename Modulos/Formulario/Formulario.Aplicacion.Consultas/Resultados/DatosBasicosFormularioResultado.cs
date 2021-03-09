using System;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class DatosBasicosFormularioResultado
    {
        public int Id { get; set; } // el Id del detalle de linea del formulario
        public int NroFormulario { get; set; }
        public int IdEstado { get; set; }
        public string SexoId { get; set; }
        public string CodigoPais { get; set; }
        public string NroDocumento { get; set; }
        public int IdNumero { get; set; }
        public int IdOrigen { get; set; }
        public string MotivoRechazo { get; set; }
        public string MotivoRechazoPrestamo { get; set; }
        public string Observaciones { get; set; }
        public string NroSticker { get; set; }
        public string ObservacionPrestamo { get; set; }

        public string NombreSucursalBancaria { get; set; }
        public string DomicilioSucursalBancaria { get; set; }
        public DateTime? FechaInicioPagos { get; set; }
        public DateTime? FechaPrimerVencimientoPago { get; set; }
        public DateTime? FechaVencimientoPlanPago { get; set; }
        public DateTime? FechaForm { get; set; }
        public string NumeroCaja { get; set; }
        public int? TipoApoderado { get; set; }
    }
}