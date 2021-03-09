using System.Collections.Generic;
using System.Web.Http;
using Formulario.Aplicacion.Servicios;
using Infraestructura.Core.Comun.Presentacion;

namespace Api.Controllers
{
    public class OrigenesFormularioController : ApiController
    {
        private readonly OrigenFormularioServicio _origenFormularioServicio;

        public OrigenesFormularioController(OrigenFormularioServicio origenFormularioServicio)
        {
            _origenFormularioServicio = origenFormularioServicio;
        }

        public IList<ClaveValorResultado<string>> Get()
        {
            return _origenFormularioServicio.ConsultarOrigenes();
        }
    }
}
