using System.ComponentModel.DataAnnotations;

namespace Configuracion.Aplicacion.Comandos
{
    public class RecursoComando
    {
        public long IdRecurso { get; set; }

        [MaxLength(100, ErrorMessage = "El nombre del recurso tiene un largo máximo de 100 caracteres.")]
        public string Nombre { get; set; }

        [MaxLength(200, ErrorMessage = "La descripción del recurso tiene un largo máximo de 200 caracteres.")]
        public string Descripcion { get; set; }

        [MaxLength(2048, ErrorMessage = "La descripción del recurso tiene un largo máximo de 2048 caracteres.")]
        public string Url { get; set; }
    }
}