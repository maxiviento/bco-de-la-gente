using System.ComponentModel.DataAnnotations;

namespace Formulario.Aplicacion.Comandos
{
    public class RequisitoChecklistComando
    {
        [Required(ErrorMessage = "El id del item del requisito es requerido.")]
        public int IdItem { get; set; }

        [Required(ErrorMessage = "El id del área del requisito es requerido.")]
        public int IdArea { get; set; }

        [Required(ErrorMessage = "El id de la etapa del requisito es requerido.")]
        public int IdEtapa { get; set; }

        [Required(ErrorMessage = "El número de orden del requisito es requerido.")]
        public int NroOrden { get; set; }
    }
}