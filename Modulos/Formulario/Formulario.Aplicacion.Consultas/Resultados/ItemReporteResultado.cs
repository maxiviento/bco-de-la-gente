namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class ItemReporteResultado : ReporteFormularioResultado
    {
        public ItemReporteResultado() { }

        public ItemReporteResultado(string nombre, bool seleccionado)
        {
            Nombre = nombre;
            Seleccionado = seleccionado;
        }
        
        public string Nombre { get; set; }
        public bool Seleccionado { get; set; }
    }
}
