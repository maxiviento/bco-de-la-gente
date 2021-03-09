
using System.Collections.Generic;

namespace Formulario.Aplicacion.Comandos
{
    public class ActualizarPrecioVentaComando
    {
        public decimal UnidadesEstimadas { get; set; }
        public string Producto { get; set; }
        public decimal? IdProducto { get; set; }
        public decimal IdEmprendimiento { get; set; }
        public IList<CostoPrecioVentaComando> Costos { get; set; }
        public IList<decimal> CostosAEliminar { get; set; }
        public decimal? GananciaEstimada { get; set; }
    }
}
