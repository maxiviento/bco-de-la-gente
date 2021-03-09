using System.ComponentModel.DataAnnotations;

namespace Configuracion.Aplicacion.Comandos
{
    public class RegistrarEtapaComando
    {
        [Required(ErrorMessage = "El nombre en la etapa es requerido")]
        [MaxLength(100, ErrorMessage = "El nombre no debe superar los 100 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción en la etapa es requerida")]
        [MaxLength(200, ErrorMessage = "La descripción no debe superar los 200 caracteres.")]
        public string Descripcion { get; set; }
    }
}