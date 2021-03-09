using System.ComponentModel.DataAnnotations;

namespace Configuracion.Aplicacion.Comandos
{
    public class RegistrarAreaComando
    {
        [Required(ErrorMessage = "El nombre del área es requerido")]
        [MaxLength(100, ErrorMessage = "El nombre del área no puede tener mas de 100 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción del área es requerida")]
        [MaxLength(200, ErrorMessage = "La descripción del área no puede tener mas de 200 caracteres.")]
        public string Descripcion { get; set; }
    }
}