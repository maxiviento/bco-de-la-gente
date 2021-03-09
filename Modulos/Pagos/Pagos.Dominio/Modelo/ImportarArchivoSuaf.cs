using System;

namespace Pagos.Dominio.Modelo
{
    public class ImportarArchivoSuaf
    {
        public int NumeroFormulario { get; set; }
        public string FechaDevengadoString { get; set; }
        public DateTime FechaDevengado { get; set; }
        public string Devengado { get; set; }
    }
}
