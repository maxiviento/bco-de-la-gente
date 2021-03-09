using System;

namespace ApiBatch.Base
{
    public class ContratoMutuo
    {
        public int CantidadFormularios { get; set; }
        public string DatosSolicitantes { get; set; }
        public string DatosGarantes { get; set; }
        public string NroFormulario { get; set; }
        public DateTime Fecha { get; set; }
        public string NombreCompletoSolicitante { get; set; }
        public string NroDocumentoSolicitante { get; set; }
        public string Cuil { get; set; }
        public string ValorPrestamo { get; set; }
        public string DomicilioCompletoSolicitante { get; set; }
        public string CantidadCuotas { get; set; }
        public string MontoCuota { get; set; }
        public DateTime? FechaPrimerVencimientoPago { get; set; }
        public string EstadoCivilSolicitante { get; set; }
        public string NombreLinea { get; set; }
        public string DescripcionLinea { get; set; }
        public string Destino { get; set; }
    }
}