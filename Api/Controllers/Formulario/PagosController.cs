using System.Threading.Tasks;
using System.Web.Http;
using Pagos.Aplicacion.Servicios;

namespace Api.Controllers.Formulario
{
    public class PagosController : ApiController
    {
        private readonly PagosServicio _pagosServicio;

        public PagosController(PagosServicio pagosServicio)
        {
            _pagosServicio = pagosServicio;
        }

        [Route("reporte")]
        [HttpGet]
        public Task<string> GetReporteFormulario([FromUri] int id)
        {
            return _pagosServicio.ObtenerReportePagos(id);
        }
    }
}
