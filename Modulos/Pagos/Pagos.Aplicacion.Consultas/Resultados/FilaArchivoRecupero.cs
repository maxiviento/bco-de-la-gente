using System;

namespace Pagos.Aplicacion.Consultas.Resultados
{
    public class FilaArchivoRecupero
    {
        public decimal IdCabecera { get; set; }
        public decimal NroFormulario { get; set; }
        public decimal NroCuota { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
    }
}
