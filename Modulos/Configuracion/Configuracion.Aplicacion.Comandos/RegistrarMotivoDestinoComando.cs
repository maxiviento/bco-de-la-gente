using System.ComponentModel.DataAnnotations;

namespace Configuracion.Aplicacion.Comandos
{
    public class RegistrarMotivoDestinoComando
    {
        [Required(ErrorMessage = "Debe seleccionar un motivo destino para la línea.")]
        public long? Id { get; set; }

        public string Descripcion { get; set; }
    }
}