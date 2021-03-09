using System;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class FormulariosSituacionResultado
    {
        public string LineaPrestamo { get; set; }
        public string OrigFormulario { get; set; }
        public string NroFormulario { get; set; }
        public DateTime FecAlta { get; set; }
        public string EstFormulario { get; set; }
        public string MotRechazoForm { get; set; }
        public string NroPrestamo { get; set; }
        public string EstPrestamo { get; set; }
        public string MotRechazoPrest { get; set; }
        public string MontoPrestamo { get; set; }
        public string CantCuotas { get; set; }
        public string CantCuotasPagadas { get; set; }
        public string IdRechFrom { get; set; }
        public string IdRechPrest { get; set; }
        public int IdFormulario { get; set; }
        public int IdPrestamo { get; set; }
        public string NumeroCaja { get; set; }
        public string SituacionGarantia { get; set; }
        public bool TienePlanCuotas { get; set; }
        public DateTime FecSeguimiento { get; set; }
        public decimal? IdEstadoFormulario { get; set; }
        public decimal? IdEstadoPrestamo { get; set; }
    }
}
