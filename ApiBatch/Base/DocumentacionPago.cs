using System;

namespace ApiBatch.Base
{
    public class DocumentacionPago
    {
        //Caratula
        public string NroFormulario { get; set; }
        public DateTime FechaInicioPagos { get; set; }
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

        //Providencia
        public string Destino { get; set; }
        public string NombreLinea { get; set; }
        public string NroSticker { get; set; }
        public string Cuil { get; set; }
        public DateTime Fecha { get; set; }

        //Pagare
        public DateTime? FechaVencimientoPlanPago { get; set; }
        public string DomicilioSucursalBancaria { get; set; }

        //Contrato mutuo
        public int CantidadFormularios { get; set; }
        public string DatosSolicitantes { get; set; }
        public string DatosGarante { get; set; }
        public string CantidadCuotas { get; set; }
        public string MontoCuota { get; set; }
        public DateTime FechaPrimerVencimientoPago { get; set; }
        public string EstadoCivilSolicitante { get; set; }

        public string NroCuota { get; set; }
        public DateTime VencimientoCuota { get; set; }
        public string IdTipoProceso { get; set; }
    }
}