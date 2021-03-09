using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Formulario.Aplicacion.Comandos
{
   public class ActualizaSucursalComando
    {
        [Required(ErrorMessage = "El id del formulario es requerido.")]
        public List<int> IdsFormularios { get; set; }
        [Required(ErrorMessage = "El id del banco es requerido.")]
        public string IdBanco { get; set; }
        [Required(ErrorMessage = "El id de la sucursal es requerido.")]
        public string IdSucursal { get; set; }
    }
}
