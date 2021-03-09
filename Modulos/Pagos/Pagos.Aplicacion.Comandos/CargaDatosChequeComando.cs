using System;

namespace Pagos.Aplicacion.Comandos
{
    public class CargaDatosChequeComando
    {
        public int IdFormulario { get; set; }
        public string NroCheque { get; set; }
        public DateTime? FechaVencimientoCheque { get; set; }
    }
}