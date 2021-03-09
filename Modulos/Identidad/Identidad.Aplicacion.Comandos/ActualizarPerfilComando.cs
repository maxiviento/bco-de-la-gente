using System.Collections.Generic;
using Infraestructura.Core.Comun.Dato;
using System.ComponentModel.DataAnnotations;

namespace Identidad.Aplicacion.Comandos
{
    public class ActualizarPerfilComando
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(200, ErrorMessage = "El nombre no puede superar los 200 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Las funcionalidades son requeridas")]
        public IList<Id> Funcionalidades { get; set; }
    }
}
