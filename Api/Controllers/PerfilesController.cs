using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Identidad.Aplicacion.Comandos;
using Identidad.Aplicacion.Consultas.Consultas;
using Identidad.Aplicacion.Consultas.Resultados;
using Identidad.Aplicacion.Servicios;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;
using Soporte.Aplicacion.Servicios;

namespace Api.Controllers
{
    public class PerfilesController : ApiController
    {
        private readonly PerfilServicio _perfilServicio;

        public PerfilesController(PerfilServicio perfilServicio)
        {
            _perfilServicio = perfilServicio;
        }

        // GET: api/Perfiles
        public Resultado<PerfilResultado> Get([FromUri] PerfilConFiltrosConsulta consulta)
        {
            if (consulta == null)
            {
                consulta = new PerfilConFiltrosConsulta { NumeroPagina = 0 };
            }

            consulta.TamañoPagina = (int.Parse(ParametrosSingleton.Instance.GetValue("4")));

            var perfiles = _perfilServicio.ConsultarConFiltrosPaginados(consulta);

            perfiles.Elementos = perfiles.Elementos
                .OrderBy(p => p.Nombre)
                .ToList();

            foreach (var perfil in perfiles.Elementos)
            {
                perfil.Activo = !perfil.FechaBaja.HasValue;
            }

            return perfiles;
        }

        // GET: api/Perfiles/Todos
        [HttpGet, Route("Todos")]
        public IList<PerfilResultado> GetTodos([FromUri] PerfilConFiltrosConsulta consulta)
        {
            if (consulta == null)
                consulta = new PerfilConFiltrosConsulta();
            consulta.IncluirBajas = false;

            var perfiles = _perfilServicio.ConsultarConFiltros(consulta)
                .OrderBy(p => p.Nombre)
                .ToList();

            return perfiles;
        }

        // GET: api/Perfiles/5
        public PerfilResultado Get(int id)
        {
            var perfil = _perfilServicio.ConsultarPorId(new Id(id));

            perfil.Activo = !perfil.FechaBaja.HasValue;

            return perfil;
        }

        // GET: api/Perfiles/Funcionalidades
        [Route("Funcionalidades")]
        public IList<FuncionalidadResultado> GetFuncionalidades()
        {
            var funcionalidades = _perfilServicio.ObtenerTodasFuncionalidades();

            return funcionalidades;
        }

        // GET: api/Perfiles/5/Funcionalidades
        [Route("{idPerfil}/Funcionalidades")]
        public IList<FuncionalidadResultado> GetFuncionalidades(int idPerfil)
        {
            var funcionalidades = _perfilServicio.ObtenerFuncionalidadesPorPerfil(new Id(idPerfil));

            return funcionalidades;
        }

        // POST: api/Perfiles
        public Id Post([FromBody] ActualizarPerfilComando comando)
        {
            var resultado = _perfilServicio.Registrar(comando);

            return resultado;
        }

        // PUT: api/Perfiles/5
        public Id Put(int id, [FromBody] ActualizarPerfilComando comando)
        {
            var resultado = _perfilServicio.Actualizar(new Id(id), comando);

            return resultado;
        }

        // Put: api/Perfiles/5/Baja
        [HttpPut, Route("{id}/Baja")]
        public Id PutBaja(int id, [FromBody] DarDeBajaPerfilComando comando)
        {
            var resultado = _perfilServicio.DarDeBaja(new Id(id), comando);

            return resultado;
        }
    }
}