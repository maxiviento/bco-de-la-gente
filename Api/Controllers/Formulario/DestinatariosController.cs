using System.Collections.Generic;
using System.Web.Http;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Aplicacion.Servicios;

namespace Api.Controllers.Formulario
{
    public class DestinatariosController : ApiController
    {
        private readonly DestinatarioServicio _destinatarioServicio;

        public DestinatariosController(DestinatarioServicio destinatarioServicio)
        {
            _destinatarioServicio = destinatarioServicio;
        }

        public IList<DestinatarioResultado> Get()
        {
            return _destinatarioServicio.ConsultarDestinatarios();
        }
    }
}