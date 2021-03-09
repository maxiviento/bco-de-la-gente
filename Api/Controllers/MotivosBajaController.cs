using Configuracion.Aplicacion.Consultas.Resultados;
using Configuracion.Aplicacion.Servicios;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Identidad.Aplicacion.Servicios;
using Infraestructura.Core.Comun.Presentacion;

namespace Api.Controllers
{
    public class MotivosBajaController : ApiController
    {
        private readonly MotivoBajaServicio _motivoBajaServicio;
        private readonly UsuarioServicio _usuarioServicio;
        private readonly PerfilServicio _perfilServicio;

        public MotivosBajaController(MotivoBajaServicio motivoBajaServicio, UsuarioServicio usuarioServicio,
            PerfilServicio perfilServicio)
        {
            _motivoBajaServicio = motivoBajaServicio;
            _usuarioServicio = usuarioServicio;
            _perfilServicio = perfilServicio;
        }

        public IEnumerable<ConsultaMotivosBajaResultado> Get()
        {
            return _motivoBajaServicio.Consultar();
        }

        // GET: api/MotivosBaja/Usuarios
        [Route("Usuarios")]
        public IEnumerable<ClaveValorResultado<string>> GetUsuarios()
        {
            var motivos = _usuarioServicio.ObtenerTodosMotivos().ToList();

            var resultado = motivos.Select(
                    motivo => new ClaveValorResultado<string>(
                        motivo.Id.Valor.ToString(),
                        motivo.Nombre))
                .ToList();

            return resultado;
        }

        // GET: api/MotivosBaja/Perfiles
        [Route("Perfiles")]
        public IEnumerable<ClaveValorResultado<string>> GetPerfiles()
        {
            var motivos = _perfilServicio.ObtenerTodosMotivos().ToList();

            var resultado = motivos.Select(
                    motivo => new ClaveValorResultado<string>(
                        motivo.Id.Valor.ToString(),
                        motivo.Nombre))
                .ToList();

            return resultado;
        }
    }
}