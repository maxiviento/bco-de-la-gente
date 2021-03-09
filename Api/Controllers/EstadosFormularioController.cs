using System.Collections.Generic;
using System.Web.Http;
using Formulario.Aplicacion.Servicios;
using Infraestructura.Core.Comun.Presentacion;

namespace Api.Controllers
{
    public class EstadosFormularioController : ApiController
    {
        private readonly EstadoFormularioServicio _estadoFormularioServicio;

        public EstadosFormularioController(EstadoFormularioServicio estadoFormularioServicio)
        {
            _estadoFormularioServicio = estadoFormularioServicio;
        }

        // GET: api/MotivosBaja/Usuarios
        [Route("Prestamos")]
        public IList<ClaveValorResultado<string>> GetPrestamos()
        {
            return _estadoFormularioServicio.ConsultarEstadosParaPrestamos();
        }

        public IList<ClaveValorResultado<string>> Get()
        {
            return _estadoFormularioServicio.ConsultarEstadosFormulario();
        }

        [HttpGet]
        [Route("consultar-estados-filtro-cambio-estado")]
        public IList<ClaveValorResultado<string>> ObtenerEstadosFiltroCambioEstado()
        {
            return _estadoFormularioServicio.ObtenerEstadosFiltroCambioEstado();
        }
    }
}
