

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class ItemsPrecioVentaResultado
    {
        public decimal? Id { get; set; }
        public decimal? IdItem { get; set; }
        public decimal? IdTipo { get; set; }
        public decimal? PrecioUnitario { get; set; }
        public decimal? ValorMensual { get; set; }
        public string Observacion { get; set; }
        public string Detalle { get; set; }
        public string Nombre { get; set; }

        public ItemsPrecioVentaResultado(decimal? id, decimal? idItem, decimal? idTipo, decimal? precioUnitario, decimal? valorMensual, string nombre)
        {
            Id = id;
            IdItem = idItem;
            IdTipo = idTipo;
            PrecioUnitario = precioUnitario;
            ValorMensual = valorMensual;
            Nombre = nombre;
        }

        public ItemsPrecioVentaResultado()
        {
        }
    }
}
