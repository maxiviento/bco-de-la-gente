using System.ComponentModel.DataAnnotations;

namespace Formulario.Aplicacion.Comandos
{
    public class RegistrarTipoInteresComando
    {
        [Required(ErrorMessage = "Debe seleccionar un tipo de interés para el detalle de la línea.")]
        public long? Id { get; set; }
        public string Descripcion { get; set; }
    }
}