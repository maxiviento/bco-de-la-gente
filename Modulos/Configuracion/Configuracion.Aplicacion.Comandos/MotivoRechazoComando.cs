using System.ComponentModel.DataAnnotations;
using Identidad.Dominio.Modelo;

namespace Configuracion.Aplicacion.Comandos
{
    public class MotivoRechazoComando
    {
        public long? Id { get; set; }

        [Required(ErrorMessage = "El nombre del motivo rechazo es requerido."),
         MaxLength(100, ErrorMessage = "El nombre del motivo de rechazo no puede superar los 100 caractereres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción del motivo rechazo es requerido."),
         MaxLength(200, ErrorMessage = "La descripción del motivo de rechazo no puede superar los 200 caractereres.")]
        public string Descripcion { get; set; }

        public string Abreviatura { get; set; }
        public bool EsAutomatico { get; set; }
        public Ambito Ambito { get; set; }
        public string Observaciones { get; set; }
        public string Codigo { get; set; }
    }
}
