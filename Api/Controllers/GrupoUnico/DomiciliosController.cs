using System.Web.Http;
using Identidad.Aplicacion.Servicios;
using Infraestructura.Core.CiDi.Api;

namespace Api.Controllers.GrupoUnico
{
    public class DomiciliosController : GrupoFamiliarControllerBase
    {
        [Route("url-consultar")]
        public string GetUrlConsultar(string idVin)
        {
            return CallApiStringUrl(idVin, ApiDomicilios.UrlConsultar);
        }

        [Route("url-consultar-caracteristicas")]
        public string GetUrlConsultarCaracteristicas(string idVin)
        {
            return CallApiStringUrl(idVin, ApiDomicilios.UrlConsultarCaracteristicas);
        }

        [Route("url-modificar-caracteristicas")]
        public string GetUrlModificarCaracteristicas(string idVin)
        {
            return CallApiStringUrl(idVin, ApiDomicilios.UrlModificarCaracteristicas);
        }

        [Route("url-alta-domicilio")]
        public string GetUrlAltaDomicilio(string sexo, string dni, string pais)
        {
            int tipoDomicilio = 4; // Modificar en caso de ser necesario
            int jurisdiccion = 1;
            return CallApiStringUrl(sexo, dni, pais, tipoDomicilio, jurisdiccion, ApiDomicilios.UrlAlta, 0);
        }

        [Route("consulta-datos-basicos-json")]
        public string GetApiConsultaDatosBasicosJson(string idVin)
        {
            return CallApiEntidadConsultaStringJson(idVin, ApiDomicilios.ApiConsultaDatosBasicosJson);
        }

        [Route("consulta-caracteristicas-domicilio-json")]
        public string GetApiConsultaCaracteristicasDomicilioJson(string idVin)
        {
            return CallApiEntidadConsultaStringJson(idVin, ApiDomicilios.ApiConsultaCaracteristicasDomicilioJson);
        }

        [Route("consulta-con-caracteristicas-json")]
        public string GetApiConsultaConCaracteristicasJson(string idVin)
        {
            return CallApiEntidadConsultaStringJson(idVin, ApiDomicilios.ApiConsultaConCaracteristicasJson);
        }

        [Route("consulta-domicilio-generado-json")]
        public string GetApiConsultaGeneradoJson(string sexo, string dni, string pais, int tipoDomicilio)
        {
            return CallApiEntidadConsultaStringJson(sexo, dni, pais, tipoDomicilio, ApiDomicilios.ApiConsultaDomicilioGeneradoJson, 0);
        }

        [Route("consulta-datos-basicos")]
        public object GetApiConsultaDatosBasicos(string idVin)
        {
            return CallApiObject(idVin, ApiDomicilios.ApiConsultaDatosBasicos);
        }

        [Route("consulta-caracteristicas-domicilio")]
        public object GetApiConsultaCaracteristicasDomicilio(string idVin)
        {
            return CallApiObject(idVin, ApiDomicilios.ApiConsultaCaracteristicasDomicilio);
        }

        [Route("consulta-con-caracteristicas")]
        public object GetApiConsultaConCaracteristicas(string idVin)
        {
            return CallApiObject(idVin, ApiDomicilios.ApiConsultaConCaracteristicas);
        }

        [Route("consulta-domicilio-generado")]
        public object GetApiConsultaGenerado(string sexo, string dni, string pais, int tipoDomicilio)
        {
            return CallApiObject(sexo, dni, pais, tipoDomicilio, ApiDomicilios.ApiConsultaDomicilioGenerado, 0);
        }
    }
}