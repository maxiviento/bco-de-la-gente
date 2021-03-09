using System;
using System.Collections.Generic;
using System.Linq;
using Identidad.Aplicacion.Consultas.Consultas;
using Identidad.Aplicacion.Consultas.Resultados;
using Identidad.Dominio.IRepositorio;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;
using Infraestructura.Core.Datos;
using NHibernate;

namespace Datos.Repositorios
{
    public class PerfilRepositorio : NhRepositorio<Perfil>, IPerfilRepositorio
    {
        public PerfilRepositorio(ISession sesion) : base(sesion)
        {
        }

        public IList<PerfilResultado> ConsultarConFiltros(PerfilConFiltrosConsulta consulta)
        {
            var perfiles = Execute("PR_OBTENER_PERFILES_PAG")
                .AddParam(new Id())
                .AddParam(consulta.Nombre)
                .AddParam(consulta.IncluirBajas)
                .ToListResult<PerfilResultado>();

            return perfiles;
        }
        public IList<PerfilResultado> ConsultarTodos()
        {
            var perfiles = Execute("PR_OBTENER_PERFILES")
               .ToListResult<PerfilResultado>();

            return perfiles;
        }

        public Resultado<PerfilResultado> ConsultarConFiltrosPaginados(PerfilConFiltrosConsulta consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            var elementosEncontrados = Execute("PR_OBTENER_PERFILES_PAG")
                .AddParam(new Id())
                .AddParam(consulta.Nombre)
                .AddParam(consulta.IncluirBajas)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<PerfilResultado>();

            var perfiles = CrearResultado(consulta, elementosEncontrados);

            return perfiles;
        }
        
        public int ObtenerValorPaginacion()
        {
            return Execute("PR_OBTENER_VALOR_PARAMETRO")
                .AddParam(4)
                .AddParam(DateTime.Now)
                .ToEscalarResult<int>();
        }

        public PerfilResultado ConsultarPorId(Id id)
        {
            var perfil = Execute("PR_OBTENER_PERFILES_PAG")
                .AddParam(id)
                .AddParam(string.Empty)
                .AddParam(true)
                .ToUniqueResult<PerfilResultado>();

            return perfil;
        }

        public bool PerfilEnUso(Id id)
        {
            return Execute("PR_PERFIL_EN_USO")
                .AddParam(id)
                .ToEscalarResult<bool>();
        }

        public PerfilResultado ConsultarPorIdUsuario(Id idUsuario)
        {
            var perfil = Execute("PR_OBTENER_PERF_X_USR_ACTUAL")
                .AddParam(idUsuario)
                .ToUniqueResult<PerfilResultado>();

            return perfil;
        }

        public IList<FuncionalidadResultado> ConsultarFuncionalidades(Id id)
        {
            var funcionalidades = Execute("PR_OBTENER_FUNCIONALIDADES")
                .AddParam(id)
                .ToListResult<FuncionalidadResultado>();

            return funcionalidades;
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
      

        public Id RegistrarPerfil(Perfil perfil)
        {
            var res = Execute("PCK_ABM_SEGURIDAD.PR_ABM_PERFIL")
                .AddParam(new Id(-1))
                .AddParam(perfil.Nombre)
                .AddParam(perfil.UsuarioAlta.Id)
                .ToSpResult();

            perfil.Id = res.Id;

            foreach (var funcionalidad in perfil.Funcionalidades)
            {
                Execute("PCK_ABM_SEGURIDAD.PR_ABM_FUNC_X_PERFIL")
                    .AddParam(perfil.Id)
                    .AddParam(funcionalidad.Id)
                    .ToSpResult();
            }

            return res.Id;
        }

        public Id ActualizarPerfil(Perfil perfil)
        {
            var res = Execute("PCK_ABM_SEGURIDAD.PR_ABM_PERFIL")
                .AddParam(perfil.Id)
                .AddParam(perfil.Nombre)
                .AddParam(perfil.UsuarioModificacion.Id)
                .AddParam(perfil.MotivoBaja?.Id)
                .ToSpResult();

            Execute("PCK_ABM_SEGURIDAD.PR_ABM_FUNC_X_PERFIL")
                .AddParam(perfil.Id)
                .AddParam(new Id()) //Borra las funcionalidades anteriores
                .ToSpResult();

            foreach (var funcionalidad in perfil.Funcionalidades)
            {
                Execute("PCK_ABM_SEGURIDAD.PR_ABM_FUNC_X_PERFIL")
                    .AddParam(perfil.Id)
                    .AddParam(funcionalidad.Id)
                    .ToSpResult();
            }

            return res.Id;
        }

        public IList<Perfil> ObtenerPerfilesPorUsuarioId(Id usuarioId)
        {
            var perfiles = Execute("PR_OBTENER_PERF_X_USR")
                .AddParam(usuarioId)
                .ToListResult<Perfil>();

            foreach (var perfil in perfiles)
            {
                var funcionalidades = Execute("PR_OBTENER_FUNC_X_PERFIL")
                    .AddParam(perfil.Id)
                    .ToListResult<Funcionalidad>();

                foreach (var funcionalidad in funcionalidades)
                {
                    perfil.Funcionalidades.Add(funcionalidad);

                }

            }


            return perfiles;
        }
    }
}
