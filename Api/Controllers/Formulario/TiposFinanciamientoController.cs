using System.Collections.Generic;
using System.Web.Http;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Aplicacion.Servicios;

namespace Api.Controllers.Formulario
{
    public class TiposFinanciamientoController : ApiController
    {
        private readonly TipoFinanciamientoServicio _tipoFinanciamientoServicio;

        public TiposFinanciamientoController(TipoFinanciamientoServicio tipoFinanciamientoServicio)
        {
            _tipoFinanciamientoServicio = tipoFinanciamientoServicio;
        }

        public IList<TipoFinanciamientoResultado> Get()
        {
            return _tipoFinanciamientoServicio.ConsultarTiposFinanciamiento();
        }
    }
}