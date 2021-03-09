namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class ItemReporteConCategoriaResultado : ReporteFormularioResultado
    {
        public ItemReporteConCategoriaResultado() { }

        public ItemReporteConCategoriaResultado(string nombre, bool seleccionado, decimal idCategoria, string nombreCategoria)
        {
            Nombre = nombre;
            Seleccionado = seleccionado;
            IdCategoria = idCategoria;
            NombreCategoria = nombreCategoria;
        }

        public string Nombre { get; set; }
        public bool Seleccionado { get; set; }
        public decimal IdCategoria { get; set; }
        public string NombreCategoria { get; set; }
    }
}
