using Formulario.Aplicacion.Consultas.Consultas;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.Modelo;
using Infraestructura.Core.Comun.Presentacion;
using Infraestructura.Core.Datos;
using NHibernate;
using Soporte.Dominio.IRepositorio;
using Soporte.Dominio.Modelo;
using System.Collections.Generic;

namespace Datos.Repositorios.Soporte
{
     public class MonitorProcesosRepositorio : NhRepositorio<DatoMonitorProcesos>, IMonitorProcesosRepositorio
    {

        public MonitorProcesosRepositorio(ISession sesion) : base(sesion)
        {
        }

        public IList<EstadoProceso> ObtenerEstadosProceso()
        {
            return Execute("PR_OBTENER_ESTADOS_PROCESO")
                .ToListResult<EstadoProceso>();
        }

        public IList<TipoProceso> ObtenerTiposProceso()
        {
            return Execute("PR_OBTENER_TIPOS_PROCESO")
                .ToListResult<TipoProceso>();
        }

        public Resultado<BandejaMonitorProcesosResultado> ObtenerProcesosPorFiltros(BandejaMonitorProcesoConsulta consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            var elementos = Execute("PR_BANDEJA_MONITOR")
                .AddParam(consulta.FechaAlta)
                .AddParam(consulta.IdsEstado)
                .AddParam(consulta.IdsTipo)
                .AddParam(consulta.IdUsuario)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)

                .ToListResult<BandejaMonitorProcesosResultado>();

            return CrearResultado(consulta, elementos);
        }

        public string ObtenerTotalizadorProcesos(BandejaMonitorProcesoConsulta consulta)
        {
            var total = Execute("PR_BANDEJA_MONITOR_TOT")
                .AddParam(consulta.FechaAlta)
                .AddParam(consulta.IdsEstado)
                .AddParam(consulta.IdsTipo)
                .AddParam(consulta.IdUsuario)
                .ToEscalarResult<string>();

            return total;
        }

        public bool ValidarEstadoGrupoBatch(int nroGrupoProceso, int idEstado)
        {
            return Execute("PR_VALIDAR_ESTADO_GPO_BATCH")
                .AddParam(nroGrupoProceso)
                .AddParam(idEstado)
                .ToEscalarResult<bool>();
        }

        public bool CancelarProceso(int nroGrupoProceso, string idUsuario)
        {
            var res = Execute("PR_ACT_GRUPO_PROCESO_BATCH")
                .AddParam(nroGrupoProceso)
                .AddParam(6)
                .AddParam(default(string))
                .AddParam(idUsuario)
                .ToSpResult();

            return res.Mensaje == "OK";
        }
    }
}
