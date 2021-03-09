namespace Formulario.Aplicacion.Comandos
{
    public class CostoPrecioVentaComando
    {
        public decimal? Id { get; set; }
        public decimal IdTipo { get; set; }
        public string Detalle { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal? ValorMensual { get; set; }
        public decimal? IdItem { get; set; }
    }
}
