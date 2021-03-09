using System;

namespace ApiBatch.Base
{
    public class Cuponera
    {
        public string NombreLinea { get; set; }
        public int NroFormulario { get; set; }
        public string NroDocumentoSolicitante { get; set; }
        public string NombreCompletoSolicitante { get; set; }
        public int NroCuota { get; set; }
        public string MontoCuota { get; set; }
        public DateTime VencimientoCuota { get; set; }
    }
}