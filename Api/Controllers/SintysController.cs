using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Api.Controllers.GrupoUnico;
using AppComunicacion.ApiModels;
using Infraestructura.Core.CiDi.Api;
using Soporte.Aplicacion.Servicios;

namespace Api.Controllers
{
    public class SintysController : GrupoFamiliarControllerBase
    {
        private readonly SintysServicio _sintysServicio;

        public SintysController(SintysServicio sintysServicio)
        {
            _sintysServicio = sintysServicio;
        }

        [HttpGet]
        [Route("reporte-sintys")]
        public string GetReporteSintysIndivudual(string sexo, string dni, string pais)
        {
            var grupoResultado = (RespuestaAPIGrupoFamiliar)CallApiObject(sexo, dni, pais, 0,
                ApiGruposFamiliares.ApiConsultaGruposConCaractPersonas);

            var hash = HttpContext.Current.Request.Cookies["CiDi"]?.Value;

            return _sintysServicio.GenerarReporteSintysIndividual(grupoResultado, hash);
        }

        [HttpGet]
        [Route("reporte-sintys-prestamo")]
        public string GetReporteSintysPrestamo(int idFormularioLinea)
        {
            var hash = HttpContext.Current.Request.Cookies["CiDi"].Value;
            return _sintysServicio.GenerarReporteSintysPrestamo(idFormularioLinea, hash);
        }
    }
}