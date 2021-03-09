using System.Web.Http;
using Infraestructura.Core.CiDi.Api;

namespace Api.Controllers.GrupoUnico
{
    public class GruposFamiliaresController : GrupoFamiliarControllerBase
    {
        [Route("url-modificar-aplicacion-interna")]
        public string GetUrlModificarAplicacionInterna(string sexo, string dni, string pais)
        {
            return CallApiStringUrl(sexo, dni, pais, ApiGruposFamiliares.UrlModificarAplicacionInterna, 0);
        }

        [Route("url-modificar-aplicacion-externa")]
        public string GetUrlConsultarGrupos(string sexo, string dni, string pais)
        {
            return CallApiStringUrl(sexo, dni, pais, ApiGruposFamiliares.UrlConsultarGrupos, 0);
        }

        [Route("consulta-grupos-con-caract-personas-json")]
        public string GetApiConsultaGruposConCaractPersonasJson(string sexo, string dni, string pais)
        {
            return CallApiStringJson(sexo, dni, pais, 0, ApiGruposFamiliares.ApiConsultaGruposConCaractPersonasJson);
        }

        [Route("consulta-grupos-json")]
        public string GetApiConsultaGruposJson(string sexo, string dni, string pais)
        {
            return CallApiStringJson(sexo, dni, pais, 0, ApiGruposFamiliares.ApiConsultaGruposJson);
        }

        [Route("consulta-grupos")]
        public object GetApiConsultaGrupos(string sexo, string dni, string pais)
        {
            return CallApiObject(sexo, dni, pais, 0, ApiGruposFamiliares.ApiConsultaGrupos);
        }

        [Route("consulta-grupos-con-caract-personas")]
        public object GetApiConsultaGruposConCaractPersonas(string sexo, string dni, string pais)
        {
            return CallApiObject(sexo, dni, pais, 0, ApiGruposFamiliares.ApiConsultaGruposConCaractPersonas);
        }
    }
}
