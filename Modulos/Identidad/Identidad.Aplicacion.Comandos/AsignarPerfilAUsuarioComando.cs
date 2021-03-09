using System.ComponentModel.DataAnnotations;
using Infraestructura.Core.Comun.Dato;

namespace Identidad.Aplicacion.Comandos
{
    public class AsignarPerfilAUsuarioComando
    {
        [Required(ErrorMessage = "Se debe seleccionar un perfil")]
        public Id PerfilId { get; set; }
    }
}
