using System.Collections.Generic;
using System.Web.Http;
using Formulario.Aplicacion.Servicios;
using Infraestructura.Core.Comun.Presentacion;

namespace Api.Controllers.Formulario
{
    public class DepartamentosController : ApiController
    {
        private readonly DepartamentoServicio _departamentoServicio;

        public DepartamentosController(DepartamentoServicio departamentoServicio)
        {
            _departamentoServicio = departamentoServicio;
        }

        public IList<ClaveValorResultado<string>> Get()
        {
            return _departamentoServicio.Consultar();
        }
    }
}