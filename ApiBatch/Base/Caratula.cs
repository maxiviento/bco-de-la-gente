using System;

namespace ApiBatch.Base
{
    public class Caratula
    {
        public string NroFormulario { get; set; }
        public DateTime? FechaInicioPagos { get; set; }
        public string NroDocumentoSolicitante { get; set; }
        public string NombreCompletoSolicitante { get; set; }
        public string ValorPrestamo { get; set; }
        public string DescripcionLinea { get; set; }
        public string DomicilioCompletoSolicitante { get; set; }
        public string NombreGarante { get; set; }
        public string TelefonoSolicitante { get; set; }
        public string TelefonoGarante { get; set; }
        public string NombreSucursal { get; set; }
        public string IdSucursal { get; set; }
        public string Localidad { get; set; }
        public string Departamento { get; set; }
    }
}