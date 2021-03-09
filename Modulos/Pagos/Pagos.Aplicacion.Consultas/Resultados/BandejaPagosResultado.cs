using System;

namespace Pagos.Aplicacion.Consultas.Resultados
{
    public class BandejaPagosResultado
    {
        public int Id { get; set; }
        public string Linea { get; set; }
        public string Departamento { get; set; }
        public string Localidad { get; set; }
        public string Origen { get; set; }
        public decimal? CantFormularios { get; set; }
        public int NroPrestamo { get; set; }
        public decimal? MontoOtorgado { get; set; }
        public DateTime FechaPedido { get; set; }
        public int IdAgrupamiento { get; set; }
        public int NroFormulario { get; set; }
        public string ApellidoYNombre { get; set; }
    }
}
