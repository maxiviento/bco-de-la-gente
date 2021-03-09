using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Api.Controllers.GrupoUnico;
using AppComunicacion.ApiModels;
using Infraestructura.Core.CiDi.Api;
using Soporte.Aplicacion.Servicios;

namespace Api.Controllers
{
    public class RentasController : GrupoFamiliarControllerBase
    {
        private readonly RentasServicio _rentasServicio;

        public RentasController(RentasServicio rentasServicio)
        {
            _rentasServicio = rentasServicio;
        }

        [HttpGet]
        [Route("reporte-grupo-familiar")]
        public Task<string> GetGenerarReporteRentasGrupoFamiliar(string sexo, string dni, string pais)
        {
            var grupoResultado = (RespuestaAPIGrupoFamiliar) CallApiObject(sexo, dni, pais, 0,
                ApiGruposFamiliares.ApiConsultaGruposConCaractPersonas);
            var hash = HttpContext.Current.Request.Cookies["CiDi"]?.Value;

            return _rentasServicio.GenerarReporteRentas(grupoResultado, hash);
        }


        [HttpGet]
        [Route("reporte-rentas")]
        public Task<string> GetGenerarReporteRentasGrupoFamiliarRentas(int idFormularioLinea, int idPrestamoRequisito)
        {
            var hash = HttpContext.Current.Request.Cookies["CiDi"]?.Value;
            return _rentasServicio.GenerarReporteRentasPorPrestamo(idFormularioLinea, hash, idPrestamoRequisito);
        }

        [HttpGet]
        [AllowAnonymous ]
        [Route("historial-rentas")]
        public string GetHistorialReporteRentas(int idHistorial)
        {
            var hash = HttpContext.Current.Request.Cookies["CiDi"]?.Value;
            return _rentasServicio.ObtenerHistorial(idHistorial, hash);
        }
    }
}