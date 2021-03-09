using System.Collections.Generic;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class ReporteHistoricoPagados
    {
        public string Linea { get; set; }
        public string Sexo { get; set; }        
        public long? Monto { get; set; }
        public string Departamento { get; set; }
        public string Localidad { get; set; }

        public IList<ReporteHistoricoPagados> crearVacio()
        {
            IList<ReporteHistoricoPagados> reporteHistoricoPagados = new List<ReporteHistoricoPagados>();

            var _reporteHistoricoPagados = new ReporteHistoricoPagados();

            _reporteHistoricoPagados.Linea = "-";
            _reporteHistoricoPagados.Sexo = "-";
            _reporteHistoricoPagados.Departamento = "-";
            _reporteHistoricoPagados.Monto = 0;
            _reporteHistoricoPagados.Localidad = "-";

            reporteHistoricoPagados.Add(_reporteHistoricoPagados);

            return reporteHistoricoPagados;
        }
    }
}
