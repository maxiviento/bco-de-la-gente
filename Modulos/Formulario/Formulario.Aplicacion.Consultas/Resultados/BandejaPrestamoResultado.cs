using System;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class BandejaPrestamoResultado
    {
        public int Id { get; set; }
        public string NroFormulario { get; set; }
        public string NroPrestamo { get; set; }
        public DateTime? FechaAltaPrestamo { get; set; }
        public string NroLinea { get; set; }
        public string NombreYApellidoSolicitante { get; set; }
        public string Cuil { get; set; }
        public string EstadoPrestamo { get; set; }
        public string Origen { get; set; }
        public string NroStricker { get; set; }
        public DatosPersonaResultado Solicitante { get; set; }
        public string EstadoFormulario { get; set; }
        public bool EsAsociativa { get; set; }
        public decimal? IdEstadoPrestamo { get; set; }
        public decimal? IdEstadoPrestamoAnt { get; set; }
        public decimal? IdFormularioLinea { get; set; }
        public string NumeroCaja { get; set; }
        public bool EsApoderado { get; set; }
        public int MontoOtorgado { get; set; }
        public decimal? IdEstadoFormulario { get; set; }
    }
}
