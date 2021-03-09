using System;
using System.ComponentModel.DataAnnotations;
using Infraestructura.Core.Formateadores.MultipartData.Infrastructure;

namespace Pagos.Aplicacion.Comandos
{
    public class ImportarArchivoRecuperoComando
    {
        [Required(ErrorMessage = "El archivo es requerido.")]
        public HttpFile Archivo { get; set; }
        public int IdTipoEntidad { get; set; }
        public DateTime FechaRecupero { get; set; }
        public int Convenio { get; set; }
    }
}
