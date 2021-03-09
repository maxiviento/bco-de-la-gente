using System.Collections.Generic;

namespace Formulario.Aplicacion.Consultas.Resultados
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
    }
}