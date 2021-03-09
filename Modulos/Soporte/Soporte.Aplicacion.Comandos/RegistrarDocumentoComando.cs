using Infraestructura.Core.Formateadores.MultipartData.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Soporte.Aplicacion.Comandos
{
    public class RegistrarDocumentoComando
    {
        [Required(ErrorMessage = "El documento es requerido.")]
        public HttpFile Documento { get; set; }

        public long IdItem { get; set; }
        public long IdFormularioLinea { get; set; }
    }
}