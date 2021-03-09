using System.Collections.Generic;
using Configuracion.Dominio.IRepositorio;
using Configuracion.Dominio.Modelo;
using Infraestructura.Core.Comun.Presentacion;
using Infraestructura.Core.Datos;
using NHibernate;

namespace Datos.Repositorios.Configuracion
{
    public class TablaDefinidasRepositorio : NhRepositorio<TablaDefinida>, ITablaDefinidasRepositorio
    {
        public TablaDefinidasRepositorio(ISession sesion) : base(sesion)
        {
        }

        public IList<TablaDefinida> ObtenerTablasDefinidas()
        {
            return Execute("pr_obtener_tablas_emprend")
                .AddParam(default(decimal?))
                .AddParam(default(string))
            .ToListResult<TablaDefinida>();
        }

        public Resultado<TablaDefinida> ObtenerTablasDefinidasPaginadas(ConsultaTablasDefinidas consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0 ? consulta.PaginaDesde + 1 : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            var elementos = Execute("pr_obtener_tablas_emprend")
                .AddParam(consulta.IdTabla)
                .AddParam(consulta.Nombre)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<TablaDefinida>();
            var resultado = CrearResultado(consulta, elementos);
            return resultado;
        }

        public TablaDefinida ObtenerDatosTablaDefinida(int id)
        {
            return Execute("pr_obtener_tablas_emprend")
                .AddParam(id)
                .AddParam(default(string))
                .ToUniqueResult<TablaDefinida>();
        }

        public Resultado<ParametroTablaDefinidaResult> ObtenerDatosTablaDefinida(ConsultaParametrosTablasDefinidas consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0 ? consulta.PaginaDesde + 1 : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            var elementos = Execute("PR_OBTENER_PARAM_T_EMPREND")
                .AddParam(consulta.IdTabla)
                .AddParam(consulta.IdParametro)
                .AddParam(consulta.IncluirDadosDeBaja)
                .AddParam(default(string))
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<ParametroTablaDefinidaResult>();
            var resultado = CrearResultado(consulta, elementos);
            return resultado;
        }

        public IList<ParametroTablaDefinidaResult> ObtenerParametrosComboTablaDefinida(ConsultaParametrosTablasDefinidas consulta)
        {

            return Execute("PR_OBTENER_PARAM_T_EMPREND")
                .AddParam(consulta.IdTabla)
                .AddParam(consulta.IdParametro)
                .AddParam(consulta.IncluirDadosDeBaja)
                .AddParam(default(string))
                .AddParam(default(decimal?))
                .AddParam(default(decimal?))
                .ToListResult<ParametroTablaDefinidaResult>();
        }

        public IList<ParametroTablaDefinidaResult> ObtenerParametrosTablaDefinida(int idTabla)
        {
            return Execute("PR_OBTENER_PARAM_T_EMPREND")
                .AddParam(idTabla)
                .AddParam(default(decimal?))
                .AddParam(default(bool?))
                .ToListResult<ParametroTablaDefinidaResult>();
        }

        public decimal Rechazar(ParametroTablaDefinida parametro)
        {
            var resultadoSp = Execute("PR_ABM_PARAMETROS_TABLA")
                .AddParam(parametro.Id)
                .AddParam(parametro.Nombre)
                .AddParam(parametro.Descripcion)
                .AddParam(parametro.IdMotivoRechazo)
                .AddParam(default(decimal))
                .AddParam(parametro.UsuarioModificacion.Id)
                .ToSpResult();
            return resultadoSp.Id.Valor;
        }

        public decimal Registrar(ParametroTablaDefinida parametro, int idTabla)
        {
            var resultadoSp = Execute("PR_ABM_PARAMETROS_TABLA")
                .AddParam(default(decimal?))
                .AddParam(parametro.Nombre)
                .AddParam(parametro.Descripcion)
                .AddParam(parametro.IdMotivoRechazo)
                .AddParam(idTabla)
                .AddParam(parametro.UsuarioModificacion.Id)
                .ToSpResult();
            return resultadoSp.Id.Valor;
        }

        public ParametroTablaDefinidaResult ObtenerParametroTablaDefinida(int id)
        {
            return Execute("PR_OBTENER_PARAM_T_EMPREND")
                .AddParam(default(decimal?))
                .AddParam(id)
                .AddParam(default(bool?))
                .ToUniqueResult<ParametroTablaDefinidaResult>();
        }
    }
}
