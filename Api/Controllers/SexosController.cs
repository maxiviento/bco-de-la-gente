using Configuracion.Aplicacion.Consultas.Resultados;
using Configuracion.Aplicacion.Servicios;
using System.Collections.Generic;
using System.Web.Http;

namespace Api.Controllers
{
    public class SexosController : ApiController
    {
        private readonly SexoServicio _sexoServicio;

        public SexosController(SexoServicio sexoServicio)
        {
            _sexoServicio = sexoServicio;
        }

        public IEnumerable<ConsultaSexosResultado> Get()
        {
            return _sexoServicio.Consultar();
        }
    }
}