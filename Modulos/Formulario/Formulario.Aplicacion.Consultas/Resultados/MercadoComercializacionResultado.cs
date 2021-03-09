using System.Collections.Generic;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class MercadoComercializacionResultado
    {
        public IList<FormaPagoResultado> FormasPago { get; set; }
        public IEnumerable<ItemsMercadoComerResultado> ItemsPorCategoria { get; set; }
        public EstimaClientesResultado EstimaClientes { get; set; }
    }
}
