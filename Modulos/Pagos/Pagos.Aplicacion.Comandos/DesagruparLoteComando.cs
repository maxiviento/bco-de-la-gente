using System.Collections.Generic;

namespace Pagos.Aplicacion.Comandos
{
    public class DesagruparLoteComando
    {
        public decimal IdLote { get; set; }
        public List<int> IdPrestamosDesagrupados { get; set; }
    }
}
