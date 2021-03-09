using System.ComponentModel.DataAnnotations;

namespace Formulario.Aplicacion.Comandos
{
    public class RegistrarItegranteSocioComando
    {
        [Required(ErrorMessage = "Debe seleccionar un tipo integrante socio para el detalle de la línea.")]
        public long? Id { get; set; }
        public string Descripcion { get; set; }
    }
}
