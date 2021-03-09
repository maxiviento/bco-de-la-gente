using System.Collections.Generic;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class ReporteCuadroPagados
    {

        public string Departamento { get; set; }
        public string Localidad { get; set; }
        public string Linea { get; set; }
        public int? CantidadF { get; set; }
        public int? CantidadM { get; set; }
        public int? MontoF { get; set; }
        public int? MontoM { get; set; }

        public IList<ReporteCuadroPagados> crearVacio()
        {
            IList<ReporteCuadroPagados> reporteCuadroPagados = new List<ReporteCuadroPagados>();

            var _reporteCuadroPagados = new ReporteCuadroPagados();

            _reporteCuadroPagados.Departamento = "-";
            _reporteCuadroPagados.Localidad = "-";
            _reporteCuadroPagados.Linea = "-";
            _reporteCuadroPagados.CantidadF = 0;
            _reporteCuadroPagados.CantidadM = 0;
            _reporteCuadroPagados.MontoF = 0;
            _reporteCuadroPagados.MontoM = 0;
            reporteCuadroPagados.Add(_reporteCuadroPagados);

            return reporteCuadroPagados;
        }
    }

 
}
