using System.ComponentModel.DataAnnotations;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Formateadores.MultipartData.Infrastructure;

namespace Pagos.Aplicacion.Comandos
{
    public class RegistrarExcelSuafComando
    {
        [Required(ErrorMessage = "El archivo SUAF es requerido.")]
        public HttpFile Archivo { get; set; }

        [Required(ErrorMessage = "El ID de lote es requerido.")]
        public Id LoteId { get; set; }
    }
}
