using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pagos.Aplicacion.Consultas.Resultados
{
    public class ReporteCuadroCreditosResultado
    {
        public string NombreLinea { get; set; }
        public int CantAEvaluar{get;set;}
        public int CantRechazados {get;set;}
        public int CantRechazadosForm { get; set; }
        public int CantRechazadosPres { get; set; }
        public int CantProyectados {get;set;}
        public int CantPagados {get;set;}


        public IList<ReporteCuadroCreditosResultado> crearVacio()
        {
            IList<ReporteCuadroCreditosResultado> reporteCuadroCreditos = new List<ReporteCuadroCreditosResultado>();
            var _reporteCuadroCreditos = new ReporteCuadroCreditosResultado();

            _reporteCuadroCreditos.NombreLinea = "-";
            _reporteCuadroCreditos.CantAEvaluar = 0;
            _reporteCuadroCreditos.CantRechazados = 0;
            _reporteCuadroCreditos.CantRechazadosForm = 0;
            _reporteCuadroCreditos.CantRechazadosPres = 0;
            _reporteCuadroCreditos.CantProyectados = 0;
            _reporteCuadroCreditos.CantPagados = 0;

            reporteCuadroCreditos.Add(_reporteCuadroCreditos);
            return reporteCuadroCreditos;

        }
    }
}
