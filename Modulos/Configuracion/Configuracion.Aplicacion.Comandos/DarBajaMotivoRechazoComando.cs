using System.ComponentModel.DataAnnotations;

namespace Configuracion.Aplicacion.Comandos
{
    public class DarBajaMotivoRechazoComando
    {
        [Required(ErrorMessage = "El motivo de rechazo es requerido")]
        public decimal Id { get; set; }

        [Required(ErrorMessage = "El motivo de baja es requerido")]
        public decimal IdMotivoBaja { get; set; }
        public decimal idAmbito { get; set; }
    }
}
