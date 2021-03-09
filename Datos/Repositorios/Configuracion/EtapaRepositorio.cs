using System.Collections.Generic;
using Configuracion.Aplicacion.Consultas;
using Configuracion.Aplicacion.Consultas.Resultados;
using Configuracion.Dominio.IRepositorio;
using Configuracion.Dominio.Modelo;
using Infraestructura.Core.Datos;
using NHibernate;
using Infraestructura.Core.Comun.Presentacion;

namespace Datos.Repositorios.Configuracion
{
    public class EtapaRepositorio : NhRepositorio<Etapa>, IEtapaRepositorio
    {
        public EtapaRepositorio(ISession sesion) : base(sesion)
        {
        }

        public decimal RegistrarEtapa(Etapa etapa)
        {
            var response = Execute("PCK_ABMS_BANCO_GENTE.PR_ABM_ETAPAS")
                .AddParam(default(decimal?))
                .AddParam(etapa.Nombre)
                .AddParam(etapa.Descripcion)
                .AddParam(default(decimal?))
                .AddParam(etapa.UsuarioAlta.Id)
                .ToSpResult();
            etapa.Id = response.Id;
            return etapa.Id.Valor;
        }

        public bool ExisteEtapaConElMismoNombre(string etapa)
        {
            var response = Execute("PR_EXISTE_ETAPA")
                .AddParam(etapa)
                .ToEscalarResult<string>();

            return response == "S";
        }

        public Resultado<EtapaResultado.Consulta> ConsultarPorNombre(ConsultaEtapas consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            var elementos = Execute("PR_OBTENER_ETAPAS")
                .AddParam(consulta.Nombre)
                .AddParam(consulta.IncluirDadosDeBaja)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<EtapaResultado.Consulta>();

            var resultado = CrearResultado(consulta, elementos);
            return resultado;
        }

        public Etapa ConsultarPorId(decimal id)
        {
            return Execute("PR_OBTENER_ETAPA_POR_ID")
                .AddParam(id)
                .ToUniqueResult<Etapa>();
        }

        public void DarDeBaja(Etapa etapa)
        {
            Execute("PCK_ABMS_BANCO_GENTE.PR_ABM_ETAPAS")
                .AddParam(etapa.Id)
                .AddParam(etapa.Nombre)
                .AddParam(etapa.Descripcion)
                .AddParam(etapa.MotivoBaja.Id)
                .AddParam(etapa.UsuarioUltimaModificacion.Id)
                .JustExecute();
        }

        public void Modificar(Etapa etapa)
        {
            Execute("PCK_ABMS_BANCO_GENTE.PR_ABM_ETAPAS")
                .AddParam(etapa.Id)
                .AddParam(etapa.Nombre)
                .AddParam(etapa.Descripcion)
                .AddParam(default(long?))
                .AddParam(etapa.UsuarioUltimaModificacion.Id)
                .JustExecute();
        }

        public IList<EtapaResultado.Consulta> ConsultarEtapas()
        {
            var result = Execute("PR_OBTENER_TABLAS_SATELITES")
                .AddParam("T_ETAPAS")
                .ToListResult<EtapaResultado.Consulta>();
            return result;
        }

        public IList<EtapaResultado.Consulta> ConsultarEtapasPorPrestamo(long idPrestamo)
        {
            var resultados = Execute("PR_OBTENER_ETAPAS_PRESTAMO")
                .AddParam(idPrestamo)
                .ToListResult<EtapaResultado.Consulta>();

            return resultados;
        }
    }
}