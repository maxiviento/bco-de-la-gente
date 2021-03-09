using System;

namespace ApiBatch.Base
{
    public class Pagare
    {
        public string NroFormulario { get; set; }
        public string NombreCompletoSolicitante { get; set; }
        public string NroDocumentoSolicitante { get; set; }
        public string ValorPrestamo { get; set; }
        public string DomicilioCompletoSolicitante { get; set; }
        public DateTime? FechaVencimientoPlanPago { get; set; }
        public string DomicilioSucursalBancaria { get; set; }
        public DateTime Fecha { get; set; }
    }
}