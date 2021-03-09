using System.ComponentModel.DataAnnotations;

namespace Formulario.Aplicacion.Comandos
{
    public class RegistrarTipoGarantiaComando
    {
        [Required(ErrorMessage = "Debe seleccionar un tipo de garantía para el detalle de la línea.")]
        public long? Id { get; set; }

        public string Descripcion { get; set; }
    }
}