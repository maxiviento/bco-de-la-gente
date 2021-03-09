using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Api.Controllers.GrupoUnico;
using AppComunicacion.ApiModels;
using Infraestructura.Core.CiDi.Api;
using Infraestructura.Core.Comun.Archivos;
using Pagos.Aplicacion.Comandos;
using Pagos.Aplicacion.Consultas.Resultados;
using Soporte.Aplicacion.Servicios;

namespace Api.Controllers
{
    public class ProvidenciaController : GrupoFamiliarControllerBase
    {
        private readonly ProvidenciaServicio _providenciaServicio;

        public ProvidenciaController(ProvidenciaServicio providenciaServicio)
        {
            _providenciaServicio = providenciaServicio;
        }

        [HttpGet]
        [Route("reporte-providencia-prestamo")]
        public ReporteResultado GenerarReporteProvidenciaPrestamo([FromUri] ProvidenciaComando consulta)
        {
            return _providenciaServicio.ObtenerReporteProvidencia(consulta);
        }

        [HttpGet]
        [Route("reporte-providencia-masiva")]
        public ReporteResultado GenerarReporteProvidenciaMasiva([FromUri] ProvidenciaComando consulta)
        {
            return _providenciaServicio.ObtenerReporteProvidenciaMasivo(consulta);
        }

    }
}