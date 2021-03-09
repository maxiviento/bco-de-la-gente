using System.Collections.Generic;
using System.Web.Http;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Aplicacion.Servicios;

namespace Api.Controllers.Formulario
{
    public class TiposGarantiaController : ApiController
    {
        private readonly TipoGarantiaServicio _tipoGarantiaServicio;

        public TiposGarantiaController(TipoGarantiaServicio tipoGarantiaServicio)
        {
            _tipoGarantiaServicio = tipoGarantiaServicio;
        }

        public IList<TipoGarantiaResultado> Get()
        {
            return _tipoGarantiaServicio.ConsultarTiposGarantias();
        }
    }
}