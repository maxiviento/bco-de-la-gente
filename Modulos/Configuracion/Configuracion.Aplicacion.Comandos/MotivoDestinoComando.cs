using System.ComponentModel.DataAnnotations;

namespace Configuracion.Aplicacion.Comandos
{
    public class MotivoDestinoComando
    {
        public long? Id { get; set; }

        [Required(ErrorMessage = "El nombre del motivo destino es requerido."),
         MaxLength(100, ErrorMessage = "El nombre del motivo de destino no puede superar los 100 caractereres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción del motivo destino es requerido."),
         MaxLength(200, ErrorMessage = "La descripción del motivo de destino no puede superar los 200 caractereres.")]
        public string Descripcion { get; set; }
    }
}