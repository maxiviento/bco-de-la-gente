namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class EditarMontoDisponibleResultado
    {
        public decimal IdMontoDisponible { get; set; }
        public string Mensaje { get; set; }
        public bool Ok { get; set; }

        public EditarMontoDisponibleResultado(decimal idMontoDiponible, string mensaje, bool ok)
        {
            IdMontoDisponible = idMontoDiponible;
            Mensaje = mensaje;
            Ok = ok;
        }
    }
}
