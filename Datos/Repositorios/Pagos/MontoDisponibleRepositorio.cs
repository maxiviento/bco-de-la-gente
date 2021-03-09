using System.Collections.Generic;
using Formulario.Aplicacion.Consultas.Consultas;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.IRepositorio;
using Formulario.Dominio.Modelo;
using Infraestructura.Core.Comun.Presentacion;
using Infraestructura.Core.Datos;
using NHibernate;

namespace Datos.Repositorios.Pagos
{
    public class MontoDisponibleRepositorio : NhRepositorio<MontoDisponible>, IMontoDisponibleRepositorio
    {
        public MontoDisponibleRepositorio(ISession sesion) : base(sesion)
        {

        }

        public decimal Registrar(MontoDisponible montoDisponible)
        {
            var resultadoSp = Execute("PCK_ABMS_BANCO_GENTE.PR_ABM_MONTOS_DISPONIBLE")
                .AddParam(default(decimal?))
                .AddParam(montoDisponible.Descripcion)
                .AddParam(montoDisponible.Monto)  //monto disponible
                .AddParam(montoDisponible.FechaInicioPago)
                .AddParam(montoDisponible.FechaFinPago)
                .AddParam(montoDisponible.FechaDepositoBancario)
                .AddParam(montoDisponible.IdBanco)
                .AddParam(montoDisponible.IdSucursal)
                .AddParam(default(decimal?)) //motivo baja
                .AddParam(montoDisponible.UsuarioAlta.Id.Valor)
                .ToSpResult();
            return resultadoSp.Id.Valor;
        }

        public MontoDisponible ObtenerPorId(decimal id)
        {
            return Execute("PR_OBTENER_MON_DISP_POR_ID")
                .AddParam(id)
                .ToUniqueResult<MontoDisponible>();
        }

        public MontoDisponible ObtenerPorNro(decimal nroMonto)
        {
            return Execute("PR_OBTENER_MON_DISP_POR_NRO")
                .AddParam(nroMonto)
                .ToUniqueResult<MontoDisponible>();
        }

        bool IMontoDisponibleRepositorio.DarDeBaja(MontoDisponible montoDisponible)
        {
            Execute("PCK_ABMS_BANCO_GENTE.PR_ABM_MONTOS_DISPONIBLE")
                .AddParam(montoDisponible.Id)
                .AddParam(montoDisponible.Descripcion)
                .AddParam(montoDisponible.Monto)  //monto disponible
                .AddParam(montoDisponible.FechaInicioPago)
                .AddParam(montoDisponible.FechaFinPago)
                .AddParam(montoDisponible.FechaDepositoBancario)
                .AddParam(montoDisponible.IdBanco)
                .AddParam(montoDisponible.IdSucursal)
                .AddParam(montoDisponible.MotivoBaja.Id.Valor) //motivo baja
                .AddParam(montoDisponible.UsuarioAlta.Id.Valor)
                .JustExecute();

            return true;
        }

        public bool ExisteMontoDisponibleConLaMismaDescripcion(string descripcion)
        {
            var existeMontoDisponible = Execute("PR_EXISTE_MONTO_DISPONIBLE")
                .AddParam(descripcion)
                .ToEscalarResult<string>();
            return existeMontoDisponible == "S";
        }

        public EditarMontoDisponibleResultado Modificar(MontoDisponible montoDisponible)
        {
            var resultado = Execute("PCK_ABMS_BANCO_GENTE.PR_ABM_MONTOS_DISPONIBLE")
                .AddParam(montoDisponible.Id)
                .AddParam(montoDisponible.Descripcion)
                .AddParam(montoDisponible.Monto)
                .AddParam(montoDisponible.FechaInicioPago)
                .AddParam(montoDisponible.FechaFinPago)
                .AddParam(montoDisponible.FechaDepositoBancario)
                .AddParam(montoDisponible.IdBanco)
                .AddParam(montoDisponible.IdSucursal)
                .AddParam(default(decimal?)) //motivo baja
                .AddParam(montoDisponible.UsuarioUltimaModificacion.Id.Valor)
                .ToMessageResult();
            
            if (resultado.Mensaje.Equals("NEGATIVO"))
            {
                return new EditarMontoDisponibleResultado(montoDisponible.Id.Valor,
                    "El saldo del monto disponible no puede ser negativo.", false);
            }
            return new EditarMontoDisponibleResultado(montoDisponible.Id.Valor, "Modificado con éxito", true);
        }

        public Resultado<BandejaMontoDisponibleResultado> ObtenerMontosDisponibleBandeja(BandejaMontoDisponibleConsulta consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            var elementos = Execute("PR_OBTENER_MONTOS_DISPONIBLE")
                .AddParam(consulta.NroMonto?? default(decimal?))
                .AddParam(consulta.FechaDesde)
                .AddParam(consulta.FechaHasta)
                .AddParam(consulta.IncluirBaja)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<BandejaMontoDisponibleResultado>();

            return CrearResultado(consulta, elementos);
        }

        public Resultado<BandejaMontoDisponibleResultado> ObtenerMontosDisponibleProNro(BandejaMontoDisponibleConsulta consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            var elementos = Execute("PR_OBTENER_MONTOS_DISPONIBLE")
                .AddParam(consulta.NroMonto ?? default(decimal?))
                .AddParam(consulta.FechaDesde)
                .AddParam(consulta.FechaHasta)
                .AddParam(consulta.IncluirBaja)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<BandejaMontoDisponibleResultado>();

            return CrearResultado(consulta, elementos);
        }

        public Resultado<MovimientoMontoResultado> ObtenerMovimientosMonto(MovimientosMontoConsulta consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            var elementos = Execute("PR_OBTENER_MOVIMIENTOS_MONTO")
                .AddParam(consulta.IdMonto)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<MovimientoMontoResultado>();

            return CrearResultado(consulta, elementos);
        }

        public IList<MontoDisponible> ConsultarMontoDisponiblesCombo()
        {
            return Execute("PR_OBTENER_TABLAS_SATELITES")
                .AddParam("T_MONTOS_DISPONIBLE")
                .ToListResult<MontoDisponible>();
        }
    }
}
