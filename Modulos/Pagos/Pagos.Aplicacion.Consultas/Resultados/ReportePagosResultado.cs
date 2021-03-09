using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infraestructura.Core.Comun.Archivos;

namespace Pagos.Aplicacion.Consultas.Resultados
{
    public class ReportePagosResultado
    {
        public ReportePagosResultado() { }

        public ReportePagosResultado(string url)
        {
            Url = url;
        }

        public ReportePagosResultado(IEnumerable<string> messages)
        {
            Errores = messages;
        }

        public ReportePagosResultado(string url, IEnumerable<string> messages)
        {
            Url = url;
            Errores = messages;
        }

        public ReportePagosResultado(string url, TipoArchivo tipoArchivo, string fileName, IEnumerable<string> messages)
        {
            Url = url;
            Errores = messages;
            Tipo = tipoArchivo.Type;
            Extension = tipoArchivo.Extension;
            FileName = fileName;
        }

        public string Url { get; private set; }
        public string Tipo { get; private set; }
        public string FileName { get; private set; }
        public string Extension { get; private set; }
        public IEnumerable<string> Errores { get; private set; }
    }
}
