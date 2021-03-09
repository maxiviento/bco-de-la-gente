using System.Collections.Generic;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class PrecioVentaResultado : ReporteFormularioResultado
    {
        public PrecioVentaResultado(string producto, decimal? unidadesEstimadas, decimal? idProducto,decimal ganancia, int orden, decimal? precioVentaUnitario)
        {
            Producto = producto;
            UnidadesEstimadas = unidadesEstimadas;
            IdProducto = idProducto;
            GananciaEstimada = ganancia;
            Orden = orden;
            PrecioVentaUnitario = precioVentaUnitario;
        }

        public PrecioVentaResultado()
        {
        }
        public decimal? PrecioVenta { get; set; }
        public string Producto { get; set; }
        public decimal? UnidadesEstimadas { get; set; }
        public decimal? IdProducto { get; set; }
        public decimal? GananciaEstimada { get; set; }
        public IList<ItemsPrecioVentaResultado> Costos { get; set; }
        public decimal? PrecioVentaUnitario { get; set; }


    }
}
