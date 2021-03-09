
using System.ComponentModel.DataAnnotations;

namespace Configuracion.Aplicacion.Comandos
{
    public class DarDeBajaComando
    {
        [Required(ErrorMessage = "El motivo de baja es requerido")]
        public decimal IdMotivoBaja { get; set; }
    }
}
