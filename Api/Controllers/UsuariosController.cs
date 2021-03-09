using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Identidad.Aplicacion.Comandos;
using Identidad.Aplicacion.Consultas.Consultas;
using Identidad.Aplicacion.Consultas.Resultados;
using Identidad.Aplicacion.Servicios;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;
using Soporte.Aplicacion.Servicios;

namespace Api.Controllers
{
    public class UsuariosController : ApiController
    {
        private readonly UsuarioServicio _usuarioServicio;
        private readonly PerfilServicio _perfilServicio;
        private readonly ISesionUsuario _sesionUsuario;
        public UsuariosController(UsuarioServicio usuarioServicio, PerfilServicio perfilServicio, ISesionUsuario sesionUsuario)
        {
            _usuarioServicio = usuarioServicio;
            _perfilServicio = perfilServicio;
            _sesionUsuario = sesionUsuario;
        }

        // GET: api/Usuarios/usuariosPrestamo
        [HttpGet, Route("UsuariosPrestamo")]
        public IEnumerable<UsuarioResultado> GetUsuariosPrestamo()
        {
            return _usuarioServicio.ConsultarUsuarios();
        }

        // GET: api/Usuarios
        public Resultado<UsuarioResultado> Get([FromUri] UsuarioConFiltrosConsulta consulta)
        {

            if (consulta == null)
            {
                consulta = new UsuarioConFiltrosConsulta { NumeroPagina = 0 };
            }

            consulta.TamañoPagina = (int.Parse(ParametrosSingleton.Instance.GetValue("4")));
            var usuarios = _usuarioServicio.ConsultarConFiltrosPaginados(consulta);

            usuarios.Elementos = usuarios.Elementos
                .OrderBy(u => u.Nombre)
                .ToList();

            foreach (var usuario in usuarios.Elementos)
            {
                usuario.Activo = !usuario.FechaBaja.HasValue;
            }

            return usuarios;
        }

        // GET: api/Usuarios/Todos
        [HttpGet, Route("Todos")]
        public IEnumerable<UsuarioResultado> GetTodos([FromUri] UsuarioConFiltrosConsulta consulta)
        {
            if (consulta == null)
                consulta = new UsuarioConFiltrosConsulta();

            consulta.IncluyeBajas = false;
            consulta.NumeroPagina = 0;
            consulta.TamañoPagina = 0;

            var usuarios = _usuarioServicio.ConsultarConFiltros(consulta)
                .OrderBy(u => u.Nombre)
                .ToList();

            foreach (var usuario in usuarios)
            {
                usuario.Activo = !usuario.FechaBaja.HasValue;
            }

            return usuarios;
        }

        // GET: api/Usuarios/5
        public UsuarioResultado Get(int id)
        {
            var usuario = _usuarioServicio.ObtenerUsuarioPorId(new Id(id));

            return usuario;
        }

        // GET: api/Usuarios/Cidi/20123456789
        [HttpGet, Route("Cidi/{cuil}")]
        public Usuario GetUsuarioCidi(string cuil)
        {
            var usuario = _usuarioServicio.ObtenerUsuarioCidi(cuil);

            return usuario;
        }

        [Route("yo")]
        public UsuarioResultado GetUsuarioAutenticado()
        {
            var usuarioAutenticado = _usuarioServicio.ObtenerUsuarioAutenticado();
            //si sale nullpointer: localStorage.clear()
            var resultado = new UsuarioResultado()
            {
                Id = usuarioAutenticado.Id,
                Apellido = usuarioAutenticado.Apellido,
                Cuil = usuarioAutenticado.Cuil,
                Email = usuarioAutenticado.Email,
                Nombre = usuarioAutenticado.Nombre,
                ReiniciarToken = false
            };
            var hash = HttpContext.Current.Request.Cookies["CiDi"].Value;
            if (!_sesionUsuario.CiDiHash.Equals(hash))
            {
                resultado.ReiniciarToken = true;
            }
            return resultado;
        }

        [Route("logueado")]
        public UsuarioResultado GetUsuarioAutenticadoCompleto()
        {
            var usuarioAutenticado = _usuarioServicio.ObtenerUsuarioAutenticado();

            //si sale nullpointer: localStorage.clear()
            PerfilResultado perfil = _usuarioServicio.ObtenerPerfilAutenticadoCompleto();
            var resultado = new UsuarioResultado()
            {
                Id = usuarioAutenticado.Id,
                Apellido = usuarioAutenticado.Apellido,
                Cuil = usuarioAutenticado.Cuil,
                Email = usuarioAutenticado.Email,
                Nombre = usuarioAutenticado.Nombre,
                NombrePerfil = perfil.Nombre,
                PerfilId = perfil.Id
            };

            return resultado;
        }

        [Route("yo/permisos")]
        public IEnumerable<FuncionalidadResultado> GetPermisos()
        {
            var usuarioAutenticado = _usuarioServicio.ObtenerUsuarioAutenticado();
            var perfil = _perfilServicio.ConsultarPerfilPorIdUsuario(usuarioAutenticado.Id);

            if (perfil == null)
            {
                return new List<FuncionalidadResultado>();
            }
            var funcionalidades = _perfilServicio.ObtenerFuncionalidadesPorPerfil(perfil.Id);
            return funcionalidades;
        }

        // POST: api/Usuarios
        public Id Post([FromBody] RegistrarUsuarioComando comando)
        {
            var resultado = _usuarioServicio.RegistrarUsuario(comando);

            return resultado;
        }

        // PUT: api/Usuarios/5
        public Id Put(int id, [FromBody] AsignarPerfilAUsuarioComando comando)
        {
            var resultado = _usuarioServicio.ActualizarPerfil(new Id(id), comando);

            return resultado;
        }

        // DELETE: api/Usuarios/5/Baja
        [Route("{id}/Baja")]
        public Id PutBaja(int id, [FromBody] DarDeBajaPerfilComando comando)
        {
            var resultado = _usuarioServicio.DarDeBaja(new Id(id), comando);

            return resultado;
        }

        [HttpGet]
        [Route("GetUsuariosParaCombo")]
        public IEnumerable<UsuarioComboResultado> GetUsuariosCombo()
        {
            return _usuarioServicio.GetUsuariosCombo();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("ObtenerUrlCerrarSesion")]
        public string ObtenerUrlCerrarSesion()
        {
            return _usuarioServicio.ObtenerUrlCerrarSesionCid();
        }
    }
}
