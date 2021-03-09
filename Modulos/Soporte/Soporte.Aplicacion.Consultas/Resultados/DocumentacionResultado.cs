using System;
using Infraestructura.Core.Comun.Dato;

namespace Soporte.Aplicacion.Consultas.Resultados
{
    public class DocumentacionResultado
    {
        public Id Id { get; set; }
        public string NombreArchivo { get; set; }
        public long IdDocumentoCidi { get; set; }
        public DateTime? FechaAlta { get; set; }
        public string Extension { get; set; }
        public string NombreUsuario { get; set; }
    }
}