using System.Collections.Generic;
using System.Web.Http;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Aplicacion.Servicios;

namespace Api.Controllers.Formulario
{
    public class TiposInteresController : ApiController
    {
        private readonly TipoInteresServicio _tipoInteresServicio;

        public TiposInteresController(TipoInteresServicio tipoInteresServicio)
        {
            _tipoInteresServicio = tipoInteresServicio;
        }

        public IList<TipoInteresResultado> Get()
        {
            return _tipoInteresServicio.ConsultarTipoIntereses();
        }
    }
}