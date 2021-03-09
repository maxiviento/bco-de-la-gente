using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Aplicacion.Servicios;
using System.Collections.Generic;
using System.Web.Http;

namespace Api.Controllers.Formulario
{
    public class DestinosFondosController : ApiController
    {
        private readonly DestinoFondosServicio _destinoFondosServicio;

        public DestinosFondosController(DestinoFondosServicio destinoFondosServicio)
        {
            _destinoFondosServicio = destinoFondosServicio;
        }

        public IList<DestinoFondoResultado> Get()
        {
            return _destinoFondosServicio.ConsultarDestinosFondos();
        }
    }
}
