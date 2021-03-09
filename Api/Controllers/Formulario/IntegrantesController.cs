using System.Collections.Generic;
using System.Web.Http;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Aplicacion.Servicios;

namespace Api.Controllers.Formulario
{
    public class IntegrantesController : ApiController
    {
        private readonly IntegranteServicio _integranteServicio;

        public IntegrantesController(IntegranteServicio integranteServicio)
        {
            _integranteServicio = integranteServicio;
        }

        public IList<IntegranteResultado> Get()
        {
            return _integranteServicio.ConsultarIntegrantes();
        }
    }
}