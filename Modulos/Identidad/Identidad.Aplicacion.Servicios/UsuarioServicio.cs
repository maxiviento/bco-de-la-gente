using System.Collections.Generic;
using System.Linq;
using Identidad.Dominio.Modelo;
using Identidad.Aplicacion.Comandos;
using Identidad.Aplicacion.Consultas.Consultas;
using Identidad.Aplicacion.Consultas.Resultados;
using Identidad.Dominio.IRepositorio;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Excepciones;
using Infraestructura.Core.Comun.Presentacion;

namespace Identidad.Aplicacion.Servicios
{
    public class UsuarioServicio
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IPerfilRepositorio _perfilRepositorio;
        private readonly ISesionUsuario _sesionUsuario;

        public UsuarioServicio(IUsuarioRepositorio usuarioRepositorio, IPerfilRepositorio perfilRepositorio, ISesionUsuario sesionUsuario)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _perfilRepositorio = perfilRepositorio;
            _sesionUsuario = sesionUsuario;
        }

        public IEnumerable<UsuarioResultado> ConsultarUsuarios()
        {
            var usuarios = _usuarioRepositorio.ConsultarUsuarios();
            return usuarios;
        }

        public IEnumerable<UsuarioResultado> ConsultarConFiltros(UsuarioConFiltrosConsulta consulta)
        {
            var cidiHash = _sesionUsuario.CiDiHash;

            var usuarios = _usuarioRepositorio.ConsultarConFiltros(cidiHash, consulta);

            return usuarios;
        }

        public Resultado<UsuarioResultado> ConsultarConFiltrosPaginados(UsuarioConFiltrosConsulta consulta)
        {
            var cidiHash = _sesionUsuario.CiDiHash;

            consulta.TamañoPagina = _usuarioRepositorio.ObtenerValorPaginacion();

            var resultado = _usuarioRepositorio.ConsultarConFiltrosPaginados(cidiHash, consulta);

            resultado.Elementos = resultado.Elementos.OrderBy(usuario => usuario.Nombre).ToList();

            return resultado;
        }

        public UsuarioResultado ObtenerUsuarioPorId(Id usuarioId)
        {
            var cidiHash = _sesionUsuario.CiDiHash;

            var usuarioGuardado = _usuarioRepositorio.ConsultarPorId(cidiHash, usuarioId);

            var perfil = _perfilRepositorio.ConsultarPorIdUsuario(usuarioGuardado.Id);

            if (perfil != null)
                usuarioGuardado.PerfilId = perfil.Id;

            return usuarioGuardado;
        }

        public Usuario ObtenerUsuarioAutenticado()
        {
            return _sesionUsuario.Usuario;
        }
        
        public PerfilResultado ObtenerPerfilAutenticadoCompleto()
        {
            return _perfilRepositorio.ConsultarPorIdUsuario(_sesionUsuario.Usuario.Id);
        }

        public Usuario ObtenerUsuarioCidi(string cuil)
        {
            var cidiHash = _sesionUsuario.CiDiHash;

            var usuario = _usuarioRepositorio.ConsultarUsuarioPorCuil(cidiHash, cuil);

            if (usuario == null)
                throw new ModeloNoValidoException("La persona no tiene usuario CiDi");

            if (!default(Id).Equals(usuario.Id))
            {
                var perfil = _perfilRepositorio.ConsultarPorIdUsuario(usuario.Id);

                if (perfil != null)
                    throw new ModeloNoValidoException("La persona buscada ya es usuario del sistema");
            }

            return usuario;
        }

        public IEnumerable<MotivoDeBaja> ObtenerTodosMotivos()
        {
            var ambito = Ambito.USUARIO;

            var motivos = _usuarioRepositorio.ObtenerTodosMotivos(ambito);

            return motivos;
        }

        public Id RegistrarUsuario(RegistrarUsuarioComando comando)
        {
            Id resultado;

            Usuario usuarioAutenticado = ObtenerUsuarioAutenticado();

            Usuario usuario = ObtenerUsuarioCidi(comando.Cuil);

            if (!default(Id).Equals(usuario.Id))
            {
                var actualizarComando = new AsignarPerfilAUsuarioComando()
                {
                    PerfilId = comando.PerfilId
                };

                resultado = ActualizarPerfil(usuario.Id, actualizarComando);
            }
            else
            {
                var perfil = new Perfil() { Id = comando.PerfilId };

                usuario.AsignarPerfil(perfil, usuarioAutenticado);

                resultado = _usuarioRepositorio.Registrar(usuario);
            }

            return resultado;
        }

        public Id ActualizarPerfil(Id id, AsignarPerfilAUsuarioComando comando)
        {
            var usuarioGuardado = ObtenerUsuarioPorId(id);

            if (usuarioGuardado == null)
                throw new ModeloNoValidoException("El usuario no existe");

            Usuario usuarioAutenticado = ObtenerUsuarioAutenticado();

            Usuario usuario = new Usuario() { Id = usuarioGuardado.Id };

            var perfil = new Perfil() { Id = comando.PerfilId };

            usuario.AsignarPerfil(perfil, usuarioAutenticado);

            var resultado = _usuarioRepositorio.Actualizar(usuario);

            return resultado;
        }

        public Id DarDeBaja(Id id, DarDeBajaPerfilComando comando)
        {
            var usuarioGuardado = ObtenerUsuarioPorId(id);

            if (usuarioGuardado == null || usuarioGuardado.FechaBaja.HasValue)
                throw new ModeloNoValidoException("El usuario no existe");

            Usuario usuarioAutenticado = ObtenerUsuarioAutenticado();

            Usuario usuario = new Usuario() { Id = usuarioGuardado.Id };

            MotivoDeBaja motivo = _usuarioRepositorio.ObtenerMotivoPorId(Ambito.USUARIO, comando.MotivoBajaId);

            usuario.DarDeBaja(usuarioAutenticado, motivo);

            var resultado = _usuarioRepositorio.Actualizar(usuario);

            return resultado;
        }

        public IEnumerable<UsuarioComboResultado> GetUsuariosCombo()
        {
            return _usuarioRepositorio.ConsultarUsuariosCombo();
        }

        public string ObtenerUrlCerrarSesionCid()
        {
            return _usuarioRepositorio.CerrarSesionUsuario();
        }
    }
}