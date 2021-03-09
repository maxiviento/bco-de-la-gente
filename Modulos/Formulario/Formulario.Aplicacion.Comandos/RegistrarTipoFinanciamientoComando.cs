using System.ComponentModel.DataAnnotations;

namespace Formulario.Aplicacion.Comandos
{
    public class RegistrarTipoFinanciamientoComando
    {
        [Required(ErrorMessage = "Debe seleccionar un tipo de financiamiento para el detalle de la línea.")]
        public long? Id { get; set; }
        public string Descripcion { get; set; }
    }
}