using System.ComponentModel.DataAnnotations;

namespace Formulario.Aplicacion.Comandos
{
   public class ActualizarDatosPrestamoComando
    {
        [Required(ErrorMessage = "El id del préstamo es requerido.")]
        public decimal IdPrestamo { get; set; }
        public long TotalFolios { get; set; }
        [MaxLength(500, ErrorMessage = "Las observaciones no pueden superar los 500 caracteres.")]
        public string Observaciones { get; set; }
        public decimal IdEstadoPrestamo { get; set; }
        public bool ActualizarEtapa { get; set; }
        public decimal IdFormularioLinea { get; set; }
    }
}
