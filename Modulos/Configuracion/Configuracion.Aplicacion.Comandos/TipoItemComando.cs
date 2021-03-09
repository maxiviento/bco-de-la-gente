
using System.ComponentModel.DataAnnotations;

namespace Configuracion.Aplicacion.Comandos
{
    public class TipoItemComando
    {
        public long? Id { get; set; }
        [Required(ErrorMessage = "El nombre del tipo de ítem es requerido."),
         MaxLength(20, ErrorMessage = "El nombre del tipo de ítem tiene un largo máximo de 20 caracteres.")]
        public string Nombre { get; set; }
    }
}
