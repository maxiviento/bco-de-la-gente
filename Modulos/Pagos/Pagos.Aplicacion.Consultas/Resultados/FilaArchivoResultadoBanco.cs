using System;

namespace Pagos.Aplicacion.Consultas.Resultados
{
    public class FilaArchivoResultadoBanco
    {
        public DateTime FechaPago { get; set; }
        public string Periodo { get; set; }
        public string Agencia { get; set; }
        public string IdBanco { get; set; }
        public decimal Importe { get; set; }
        public string NroDocumento { get; set; }
        public string FormaPago { get; set; }
    }
}
