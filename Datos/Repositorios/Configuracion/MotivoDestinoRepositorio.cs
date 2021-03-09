using System.Collections.Generic;
using Configuracion.Aplicacion.Consultas;
using Configuracion.Aplicacion.Consultas.Resultados;
using Configuracion.Dominio.IRepositorio;
using Configuracion.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;
using Infraestructura.Core.Datos;
using NHibernate;

namespace Datos.Repositorios.Configuracion
{
    public class MotivoDestinoRepositorio : NhRepositorio<MotivoDestino>, IMotivoDestinoRepositorio
    {
        public MotivoDestinoRepositorio(ISession sesion) : base(sesion)
        {
        }

        public IList<MotivoDestino> ConsultarMotivosDestino()
        {
            var result = Execute("PR_OBTENER_TABLAS_SATELITES")
                .AddParam("T_MOTIVOS_DESTINO")
                .ToListResult<MotivoDestino>();
            return result;
        }

        public bool ExisteMotivoDestinoConMismoNombre(string nombre)
        {
            var existeArea = Execute("PR_EXISTE_MOTIVO_DESTINO")
                .AddParam(nombre)
                .ToEscalarResult<string>();
            return existeArea == "S";
        }

        public decimal RegistrarMotivoDestino(MotivoDestino motivo)
        {
            var resultadoSp = Execute("PCK_ABMS_BANCO_GENTE.PR_ABM_MOTIVOS_DESTINO")
                .AddParam(default(decimal?))
                .AddParam(motivo.Nombre)
                .AddParam(motivo.Descripcion)
                .AddParam(default(decimal?))
                .AddParam(motivo.UsuarioAlta.Id)
                .ToSpResult();
            return resultadoSp.Id.Valor;
        }

        public MotivoDestino ConsultarPorId(Id id)
        {
            return Execute("PR_OBTENER_MOT_DEST_POR_ID")
                .AddParam(id)
                .ToUniqueResult<MotivoDestino>();
        }

        public void DarDeBaja(MotivoDestino motivo)
        {
            Execute("PCK_ABMS_BANCO_GENTE.PR_ABM_MOTIVOS_DESTINO")
                .AddParam(motivo.Id)
                .AddParam(motivo.Nombre)
                .AddParam(motivo.Descripcion)
                .AddParam(motivo.MotivoBaja.Id)
                .AddParam(motivo.UsuarioUltimaModificacion.Id)
                .JustExecute();
        }

        public void Modificar(MotivoDestino motivo)
        {
            Execute("PCK_ABMS_BANCO_GENTE.PR_ABM_MOTIVOS_DESTINO")
                .AddParam(motivo.Id)
                .AddParam(motivo.Nombre)
                .AddParam(motivo.Descripcion)
                .AddParam(default(long?))
                .AddParam(motivo.UsuarioUltimaModificacion.Id)
                .JustExecute();
        }

        public Resultado<MotivoDestinoResultado.Grilla> Consultar(ConsultaMotivoDestino consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            var elementos = Execute("PR_OBTENER_MOTIVOS_DESTINO")
                .AddParam(consulta.Nombre)
                .AddParam(consulta.IncluirDadosDeBaja)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<MotivoDestinoResultado.Grilla>();
            var resultado = CrearResultado(consulta, elementos);
            return resultado;
        }
    }
}