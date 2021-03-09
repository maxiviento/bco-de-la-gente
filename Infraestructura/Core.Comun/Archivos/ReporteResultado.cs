using System.Collections.Generic;

namespace Infraestructura.Core.Comun.Archivos
{
    public class ReporteResultado
    {
        public ReporteResultado(ArchivoBase64 archivo)
        {
            Archivos = new List<ArchivoBase64>() { archivo };
        }

        public ReporteResultado(IEnumerable<ArchivoBase64> archivos)
        {
            Archivos = archivos;
        }

        public ReporteResultado(IEnumerable<string> messages)
        {
            Errores = messages;
        }

        public ReporteResultado(string message)
        {
            Errores = new List<string>() { message };
        }

        public ReporteResultado(ArchivoBase64 archivo, IEnumerable<string> messages)
        {
            Archivos = new List<ArchivoBase64>() { archivo };
            Errores = messages;
        }

        public ReporteResultado(IEnumerable<ArchivoBase64> archivos, IEnumerable<string> messages)
        {
            Archivos = archivos;
            Errores = messages;
        }

        public IEnumerable<ArchivoBase64> Archivos { get; private set; }
        public IEnumerable<string> Errores { get; private set; }
    }
}
