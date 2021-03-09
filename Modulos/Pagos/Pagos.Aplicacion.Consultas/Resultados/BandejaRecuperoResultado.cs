using System;

namespace Pagos.Aplicacion.Consultas.Resultados
{
    public class BandejaRecuperoResultado
    {
        public decimal? IdCabecera { get; set; }
        public string Entidad { get; set; }
        public DateTime FechaRecepcion { get; set; }
        public string NombreArchivo { get; set; }
        public decimal? CantTotal { get; set; }
        public decimal? CantProc { get; set; }
        public decimal? CantEspec { get; set; }
        public decimal? CantIncons { get; set; }
    }
}
