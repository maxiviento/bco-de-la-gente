using System.Collections.Generic;
using System.Web.Http;
using Formulario.Aplicacion.Servicios;
using Infraestructura.Core.Comun.Presentacion;

namespace Api.Controllers.Formulario
{
    public class LocalidadesController : ApiController
    {
        private readonly LocalidadServicio _localidadServicio;

        public LocalidadesController(LocalidadServicio localidadServicio)
        {
            _localidadServicio = localidadServicio;
        }

        [Route("{idDepartamento:decimal}")]
        public IList<ClaveValorResultado<string>> Get(decimal? idDepartamento)
        {
            return _localidadServicio.ConsultarLocalidades(idDepartamento);
        }
    }
}