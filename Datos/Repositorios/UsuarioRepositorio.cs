using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Identidad.Aplicacion.Consultas.Consultas;
using Identidad.Aplicacion.Consultas.Resultados;
using Identidad.Dominio.IRepositorio;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.CiDi.Api;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;
using Infraestructura.Core.Datos;
using NHibernate;

namespace Datos.Repositorios
{
    public class UsuarioRepositorio : NhRepositorio<Usuario>, IUsuarioRepositorio
    {
        public UsuarioRepositorio(ISession sesion) : base(sesion)
        {
        }

        public IEnumerable<UsuarioResultado> ConsultarUsuarios()
        {
                var usuarios = Execute("PR_OBTENER_USUARIOS_COMBO")
                .ToListResult<UsuarioResultado>();
            return usuarios;
        }

        public IEnumerable<UsuarioResultado> ConsultarConFiltros(string cidiHash, UsuarioConFiltrosConsulta consulta)
        {
            var usuarios = Execute("PR_OBTENER_USUARIOS_PAG")
                .AddParam(new Id())
                .AddParam(consulta.PerfilId)
                .AddParam(consulta.Cuil)
                .AddParam(consulta.IncluyeBajas)
                .AddParam(consulta.PaginaDesde)
                .AddParam(consulta.PaginaHasta)
                .ToListResult<UsuarioResultado>();

            foreach (var usuario in usuarios)
            {
                var usuarioCidi = ObtenerUsuarioCidi(cidiHash, usuario.Cuil);

                usuario.Nombre = usuarioCidi.Nombre;
                usuario.Apellido = usuarioCidi.Apellido;
                usuario.Email = usuarioCidi.Email;
            }

            return usuarios;
        }

        public Resultado<UsuarioResultado> ConsultarConFiltrosPaginados(string cidiHash,
            UsuarioConFiltrosConsulta consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            var elementosEncontrados = Execute("PR_OBTENER_USUARIOS_PAG")
                .AddParam(new Id())
                .AddParam(consulta.PerfilId)
                .AddParam(consulta.Cuil)
                .AddParam(consulta.IncluyeBajas)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<UsuarioResultado>();
            
            foreach (var usuario in elementosEncontrados)
            {
                if (usuario.Sistema != null && usuario.Sistema.Value)
                {
                    usuario.Nombre = "SISTEMA";
                    usuario.Apellido = "SISTEMA";
                    usuario.NombrePerfil = "SISTEMA";
                }
                else
                {
                    var usuarioCidi = ObtenerUsuarioCidi(cidiHash, usuario.Cuil);

                    if (usuarioCidi == null)
                    {
                        continue;
                    }
                    usuario.Nombre = usuarioCidi.Nombre;
                    usuario.Apellido = usuarioCidi.Apellido;
                    usuario.Email = usuarioCidi.Email;
                }
            }

            var usuarios = CrearResultado(consulta, elementosEncontrados);

            return usuarios;
        }

        public UsuarioResultado ConsultarPorId(string cidiHash, Id usuarioId)
        {
            var usuario = Execute("PR_OBTENER_USUARIOS_PAG")
                .AddParam(usuarioId)
                .AddParam(new Id(-1))
                .AddParam(string.Empty)
                .AddParam(true)
                .AddParam(0)
                .AddParam(10)
                .ToUniqueResult<UsuarioResultado>();

            if (usuario == null)
                return null;

            var usuarioCiDi = ObtenerUsuarioCidi(cidiHash, usuario.Cuil);

            usuario.Nombre = usuarioCiDi.Nombre;
            usuario.Apellido = usuarioCiDi.Apellido;
            usuario.Email = usuarioCiDi.Email;

            return usuario;
        }

        public Usuario ConsultarUsuarioPorCuil(string cidiHash, string cuil)
        {
            var usuarioCiDi = ApiCuenta.ObtenerUsuarioPorCuil(cidiHash, cuil);

            if (string.IsNullOrEmpty(usuarioCiDi.CUIL)) return null;

            var usuarioGuardado = Execute("PR_OBTENER_USUARIO")
                .AddParam((Id?)null)
                .AddParam(cuil)
                .ToUniqueResult<Usuario>();

            var usuario = new Usuario()
            {
                Apellido = usuarioCiDi.Apellido,
                Nombre = usuarioCiDi.Nombre,
                Email = usuarioCiDi.Email,
                Cuil = usuarioCiDi.CUIL
            };

            if (usuarioGuardado != null)
            {
                usuario.Id = usuarioGuardado.Id;
            }

            return usuario;
        }

        public UsuarioResultado ObtenerUsuarioCidi(string cidiHash, string cuil)
        {
            var usuarioCiDi = ApiCuenta.ObtenerUsuarioPorCuil(cidiHash, cuil);

            if (string.IsNullOrEmpty(usuarioCiDi.CUIL))
                return null;

            UsuarioResultado usuario = new UsuarioResultado()
            {
                Cuil = usuarioCiDi.CUIL,
                Nombre = usuarioCiDi.Nombre,
                Apellido = usuarioCiDi.Apellido,
                Email = usuarioCiDi.Email
            };

            return usuario;
        }

        public IEnumerable<MotivoDeBaja> ObtenerTodosMotivos(Ambito ambito)
        {
            var motivos = Execute("PR_OBTENER_MOT_BAJA")
                .AddParam(ambito.Id)
                .ToListResult<MotivoDeBaja>();

            return motivos;
        }

        public MotivoDeBaja ObtenerMotivoPorId(Ambito ambito, Id id)
        {
            var motivos = Execute("PR_OBTENER_MOT_BAJA")
                .AddParam(ambito.Id)
                .ToListResult<MotivoDeBaja>();

            return motivos.First(x => x.Id.Valor == id.Valor);
        }

        public Id Registrar(Usuario usuario)
        {
            var res = Execute("PCK_ABM_SEGURIDAD.PR_ABM_USUARIO_X_CUIL")
                .AddParam(usuario.Cuil)
                .ToSpResult();

            usuario.Id = res.Id;

            Actualizar(usuario);

            return res.Id;
        }

        public Id Actualizar(Usuario usuario)
        {
            var res = Execute("PCK_ABM_SEGURIDAD.PR_ABM_PERFIL_X_USUARIO")
                .AddParam(usuario.Id)
                .AddParam(usuario.Perfil?.Id)
                .AddParam(usuario.UsuarioModificacion.Id)
                .AddParam(usuario.MotivoBaja?.Id)
                .ToSpResult();

            return usuario.Id;
        }

        public IEnumerable<UsuarioComboResultado> ConsultarUsuariosCombo()
        {
            var usuarios = Execute("PR_OBTENER_USUARIOS_COMBO")
                .ToListResult<UsuarioComboResultado>();
            return usuarios;
        }

        public int ObtenerValorPaginacion()
        {
            return Execute("PR_OBTENER_VALOR_PARAMETRO")
                .AddParam(4)
                .AddParam(DateTime.Now)
                .ToEscalarResult<int>();
        }

        public string CerrarSesionUsuario()
        {
            return ApiCuenta.CerrarSesionCidi();
        }
    }
}