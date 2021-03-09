using System.ComponentModel.DataAnnotations;
using Infraestructura.Core.Comun.Dato;

namespace Identidad.Aplicacion.Comandos
{
    public class DarDeBajaPerfilComando
    {
        [Required(ErrorMessage = "Se debe seleccionar un motivo de baja")]
        public Id MotivoBajaId { get; set; }
    }
}
