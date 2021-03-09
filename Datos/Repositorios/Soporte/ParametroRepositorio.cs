using System;
using System.Collections.Generic;
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
    public class ParametroRepositorio : NhRepositorio<VigenciaParametro>, IParametrosRepositorio
    {
        public ParametroRepositorio(ISession sesion) : base(sesion)
        {
        }

        public Resultado<ConsultarParametrosResultado> ConsultarPorFiltros(ConsultaParametro consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            var elementos = Execute("PR_OBTENER_PARAMETROS")
                .AddParam(consulta.Id)
                .AddParam(consulta.IncluirNoVigentes)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<ConsultarParametrosResultado>();

            var resultado = CrearResultado(consulta, elementos);

            return resultado;
        }

        public VigenciaParametro RegistrarVigenciaParametro(VigenciaParametro parametro)
        {
            var spResutl = Execute("PR_REGISTRA_VIGENCIA_PARAM")
                .AddParam(parametro.IdParametro)
                .AddParam(parametro.FechaDesde)
                .AddParam(parametro.Valor)
                .AddParam((long)parametro.UsuarioModificacion.Id.Valor)
                .ToSpResult();

            var vigenciaParametro = new VigenciaParametro();
            vigenciaParametro.IdVigenciaParametro = (long)spResutl.Id.Valor;
            return vigenciaParametro;
            //return new VigenciaParametro();
        }

        public VigenciaParametro ActualizarVigenciaExistente(VigenciaParametro parametro)
        {
            var spResutl = Execute("PR_ACTUALIZA_VIGENCIA_PARAM")
                .AddParam(parametro.IdVigenciaParametro)
                .AddParam(parametro.Valor)
                .AddParam((long) parametro.UsuarioModificacion.Id.Valor)
                .ToSpResult();

            var vigenciaParametro = new VigenciaParametro();
            vigenciaParametro.IdVigenciaParametro = (long) spResutl.Id.Valor;
            return vigenciaParametro;
            //return new VigenciaParametro();
        }

        public VigenciaParametro ObtenerVigenciaParametroPorId(long idParametro)
        {
            return Execute("")
                .AddParam(idParametro)
                .ToUniqueResult<VigenciaParametro>();
        }

        public VigenciaParametro ObtenerValorVigenciaParametroPorFecha(Id idParametro, DateTime? fecha)
        {
            var resultado = Execute("PR_OBTENER_VALOR_PARAMETRO")
                .AddParam(idParametro)
                .AddParam(fecha)
                .ToUniqueResult<VigenciaParametro>();
            return resultado;
        }

        public ConsultarParametrosResultado ExisteVigenciaEnFecha(Id idParametro, DateTime? fechaDesde)
        {
            return Execute("PR_EXISTE_VIGENCIA")
                .AddParam(idParametro)
                .AddParam(fechaDesde)
                .ToUniqueResult<ConsultarParametrosResultado>();
        }

        #region Tablas Satelite

        public IEnumerable<Parametro> ObtenerParametros()
        {
            return ObtenerTodos<Parametro>("T_PARAMETROS");
        }

        #endregion
    }
}