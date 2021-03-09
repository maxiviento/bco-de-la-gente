using Formulario.Aplicacion.Consultas.Consultas;
using Formulario.Aplicacion.Consultas.Resultados;
using Infraestructura.Core.Comun.Archivos;
using Infraestructura.Core.Comun.Presentacion;
using Soporte.Aplicacion.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Infraestructura.Core.CiDi;

namespace Api.Controllers
{
    public class MonitorProcesosController : ApiController
    {
        private readonly MonitorProcesosServicio _monitorProcesosServicio;

        public MonitorProcesosController(MonitorProcesosServicio monitorProcesosServicio)
        {
            _monitorProcesosServicio = monitorProcesosServicio;
        }


        [Route("obtener-estados")]
        [HttpGet]
        public IList<ClaveValorResultado<string>> ObtenerEstados() { 
            return _monitorProcesosServicio.ObtenerEstados();
        }

 
        [Route("obtener-tipos")]
        [HttpGet]
        public IList<ClaveValorResultado<string>> ObtenerTipos()
        {
            return _monitorProcesosServicio.ObtenerTipos();
        }

        [Route("consultar-bandeja")]
        public Resultado<BandejaMonitorProcesosResultado> ObtenerProceso([FromBody] BandejaMonitorProcesoConsulta comando)
        {
            return _monitorProcesosServicio.ObtenerProcesoPorFiltros(comando);
        }

        [Route("consultar-totalizador")]
        public string ObtenerTotalizador([FromBody] BandejaMonitorProcesoConsulta consulta)
        {
            return _monitorProcesosServicio.ObtenerTotalizadorProceso(consulta);
        }

        [Route("descargar")]
        public ArchivoBase64 DescargarManual([FromBody] BandejaMonitorProcesosResultado resultado)
        {
            return _monitorProcesosServicio.DescargarReporte(resultado.PathArchivo);
        }

        [Route("cancelar-proceso/{idProceso}")]
        [HttpDelete]
        public string CancelarProceso(int idProceso)
        {
            var idUsuario = User.Identity.UsuarioId();

            return _monitorProcesosServicio.CancelarProceso(idProceso, idUsuario);
        }
    }
}