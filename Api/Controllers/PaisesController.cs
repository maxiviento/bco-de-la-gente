using Configuracion.Aplicacion.Consultas.Resultados;
using Configuracion.Aplicacion.Servicios;
using System.Collections.Generic;
using System.Web.Http;

namespace Api.Controllers
{
    public class PaisesController : ApiController
    {
        private readonly PaisServicio _paisServicio;

        public PaisesController(PaisServicio paisServicio)
        {
            _paisServicio = paisServicio;
        }

        public IEnumerable<ConsultaPaisesResultado> Get()
        {
            return _paisServicio.Consultar();
        }
    }
}