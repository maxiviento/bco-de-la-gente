using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Aplicacion.Servicios;
using System.Collections.Generic;
using System.Web.Http;

namespace Api.Controllers.Formulario
{
    public class CursosController : ApiController
    {
        private readonly CursoServicio _cursoServicio;

        public CursosController(CursoServicio cursoServicio)
        {
            _cursoServicio = cursoServicio;
        }

        public IEnumerable<ConsultarCursosResultado> Get()
        {
            return _cursoServicio.Consultar();
        }
    }
}