using System.ComponentModel.DataAnnotations;

namespace Formulario.Aplicacion.Comandos
{
    public class RegistrarSexoDestinatarioComando
    {
        [Required(ErrorMessage = "Debe seleccionar el sexo destinatario para la línea.")]
        public long? Id { get; set; }
        public string Descripcion { get; set; }
    }
}