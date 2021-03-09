using Formulario.Aplicacion.Consultas.Consultas;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.Modelo;
using Infraestructura.Core.Comun.Presentacion;
using System.Collections.Generic;

namespace Soporte.Dominio.IRepositorio
{
    public interface IMonitorProcesosRepositorio
    {
        IList<EstadoProceso> ObtenerEstadosProceso();
        IList<TipoProceso> ObtenerTiposProceso();
        Resultado<BandejaMonitorProcesosResultado> ObtenerProcesosPorFiltros(BandejaMonitorProcesoConsulta consulta);
        string ObtenerTotalizadorProcesos(BandejaMonitorProcesoConsulta consulta);
        bool ValidarEstadoGrupoBatch(int nroGrupoProceso, int idEstado);
        bool CancelarProceso(int nroGrupoProceso, string idUsuario);
    }
}
