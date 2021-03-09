using System.ComponentModel.DataAnnotations;

namespace Infraestructura.Core.Comun.Presentacion
{
    public class DarDeBajaComando
    {
        [Required(ErrorMessage = "El motivo es requerido para dar de baja")]
        [MaxLength(200,ErrorMessage = "El motivo de la baja no puede superar los 200 caracteres")]
        [MinLength(3,ErrorMessage = "El motivo de baja tiene que contener una longitud mayor.")]
        public  string MotivoBaja { get; set; }
    }
}