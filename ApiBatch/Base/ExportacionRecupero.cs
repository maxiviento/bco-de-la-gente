using System.Collections.Generic;

namespace ApiBatch.Base
{
    public class ExportacionRecupero
    {
        public string Entidad { get; set; }
        private string _convenioRecupero;
        public string ConvenioRecupero
        {
            get
            {
                if (_convenioRecupero == "184")
                {
                    return _convenioRecupero.PadLeft(4, '0');
                }
                return _convenioRecupero;
            }
            set
            {
                _convenioRecupero = value;
            }
        }
        public string FechaRecupero { get; set; }
        public string MontoRecuperado { get; set; }
        public string MontoRechazado { get; set; }
        public static IList<ExportacionRecupero> CrearVacio()
        {
            return new List<ExportacionRecupero>
            {
                new ExportacionRecupero
                {
                    Entidad = "-",
                    ConvenioRecupero = "-",
                    FechaRecupero = "-",
                    MontoRecuperado = "-",
                    MontoRechazado = "-"
                }
            };
        }
    }
}