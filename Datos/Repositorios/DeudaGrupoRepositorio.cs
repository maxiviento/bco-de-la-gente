using System.Collections.Generic;
using Formulario.Aplicacion.Consultas.Resultados;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;
using Infraestructura.Core.Datos;
using NHibernate;
using Soporte.Aplicacion.Consultas;
using Soporte.Aplicacion.Consultas.Resultados;
using Soporte.Dominio.IRepositorio;
using Soporte.Dominio.Modelo;

namespace Datos.Repositorios.Soporte
{
    public class DeudaGrupoRepositorio : NhRepositorio<DatoSintys>, IDeudaGrupoRepositorio
    {
        public DeudaGrupoRepositorio(ISession sesion) : base(sesion)
        {
        }

        public decimal RegistrarCabeceraHistorial(int idPrestamo, Id idUsuario, string nombreLinea)
        {
            var resultado = Execute("PR_REGISTRA_HIST_DEUDA")
                .AddParam(idPrestamo)
                .AddParam(idUsuario)
                .AddParam(nombreLinea)
                .ToSpResult();
            return resultado.Id.Valor;
        }

        public decimal RegistrarDetalleHistorial(string nroPrestamo, Id id)
        {
            var resultado = Execute("PR_REGISTRA_DATOS_SINTYS")
                .AddParam(nroPrestamo)
                .AddParam(id)
                .ToSpResult();
            return resultado.Id.Valor;
        }

        public Resultado<DocumentacionResultado> ObtenerTodosHistorialesDeudaGrupo(DocumentacionConsulta consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            var elementos = Execute("PR_OBTENER_DATOS_DEUDA_GRUPO")
                .AddParam(consulta.IdPrestamo)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<DocumentacionResultado>();
            return CrearResultado(consulta, elementos);
        }

        public List<CabeceraHistorialDeudaGrupo> ObtenerCabeceraHistorialDeudaGrupo(decimal idHistorial)
        {
            return (List<CabeceraHistorialDeudaGrupo>)
                Execute("PR_OBTENER_HIST_DEUDA_CAB")
                    .AddParam(idHistorial)
                    .ToListResult<CabeceraHistorialDeudaGrupo>();
        }

        public List<ReporteDeudaGrupoConvivienteResultado> ObtenerDetalleHistorialSintys(decimal idHistorial)
        {
            return (List<ReporteDeudaGrupoConvivienteResultado>)
                Execute("PR_OBTENER_DET_HIST_DEUDA")
                    .AddParam(idHistorial)
                    .ToListResult<ReporteDeudaGrupoConvivienteResultado>();
        }

    }
}