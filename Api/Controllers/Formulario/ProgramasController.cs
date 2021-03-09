using System.Collections.Generic;
using System.Web.Http;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Aplicacion.Servicios;

namespace Api.Controllers.Formulario
{
    public class ProgramasController : ApiController
    {
        private readonly ProgramaServicio _programaServicio;

        public ProgramasController(ProgramaServicio programaServicio)
        {
            _programaServicio = programaServicio;
        }

        public IList<ProgramaResultado> Get()
        {
            return _programaServicio.ConsultarProgramas();
        }
    }
}