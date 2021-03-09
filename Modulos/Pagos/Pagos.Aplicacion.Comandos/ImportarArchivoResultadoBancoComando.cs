using System.ComponentModel.DataAnnotations;
using Infraestructura.Core.Formateadores.MultipartData.Infrastructure;

namespace Pagos.Aplicacion.Comandos
{
    public class ImportarArchivoResultadoBancoComando
    {
        [Required(ErrorMessage = "El archivo es requerido.")]
        public HttpFile Archivo { get; set; }
    }
}
