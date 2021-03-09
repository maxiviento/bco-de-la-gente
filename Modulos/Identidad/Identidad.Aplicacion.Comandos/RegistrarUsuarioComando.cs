using System.ComponentModel.DataAnnotations;
using Infraestructura.Core.Comun.Dato;

namespace Identidad.Aplicacion.Comandos
{
    public class RegistrarUsuarioComando
    {
        [Required(ErrorMessage = "El CUIL es requerido")]
        [MaxLength(11, ErrorMessage = "El cuil no puede tener más de 11 caracteres")]
        public string Cuil { get; set; }

        [Required(ErrorMessage = "Se debe seleccionar un perfil")]
        public Id PerfilId { get; set; }
    }
}
