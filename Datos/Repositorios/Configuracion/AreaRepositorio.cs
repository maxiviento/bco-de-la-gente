using Configuracion.Aplicacion.Consultas;
using Configuracion.Aplicacion.Consultas.Resultados;
using Configuracion.Dominio.IRepositorio;
using Configuracion.Dominio.Modelo;
using Infraestructura.Core.Datos;
using NHibernate;
using System.Collections.Generic;
using Infraestructura.Core.Comun.Presentacion;

namespace Datos.Repositorios.Configuracion
{
    public class AreaRepositorio : NhRepositorio<Area>, IAreaRepositorio
    {
        public AreaRepositorio(ISession sesion) : base(sesion)
        {
        }

        public decimal RegistrarArea(Area area)
        {
            var resultadoSp = Execute("PCK_ABMS_BANCO_GENTE.PR_ABM_AREAS")
                .AddParam(default(decimal?))
                .AddParam(area.Nombre)
                .AddParam(area.Descripcion)
                .AddParam(default(decimal?))
                .AddParam(area.UsuarioAlta.Id)
                .ToSpResult();
            area.Id = resultadoSp.Id;
            return area.Id.Valor;
        }

        public bool ExisteAreaConMismoNombre(string nombre)
        {
            var existeArea = Execute("PR_EXISTE_AREA")
                .AddParam(nombre)
                .ToEscalarResult<string>();
            return existeArea == "S";
        }

        public Resultado<AreaResultado.Consulta> Consultar(ConsultarAreas consultarAreas)
        {
            consultarAreas.TamañoPagina++;

            var paginaDesde = consultarAreas.PaginaDesde == 0 ? consultarAreas.PaginaDesde + 1 : consultarAreas.PaginaDesde - consultarAreas.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consultarAreas.PaginaHasta : consultarAreas.PaginaHasta - consultarAreas.NumeroPagina;

            var elementos = Execute("PR_OBTENER_AREAS")
                .AddParam(consultarAreas.Nombre)
                .AddParam(consultarAreas.IncluirDadosDeBaja)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<AreaResultado.Consulta>();
            var resultado = CrearResultado(consultarAreas, elementos);
            return resultado;
        }

        public Area ConsultarPorId(decimal id)
        {
            return Execute("PR_OBTENER_AREA_POR_ID")
                .AddParam(id)
                .ToUniqueResult<Area>();
        }

        public void DarDeBaja(Area area)
        {
            Execute("PCK_ABMS_BANCO_GENTE.PR_ABM_AREAS")
                .AddParam(area.Id)
                .AddParam(area.Nombre)
                .AddParam(area.Descripcion)
                .AddParam(area.MotivoBaja.Id)
                .AddParam(area.UsuarioUltimaModificacion.Id)
                .JustExecute();
        }

        public void Modificar(Area area)
        {
            Execute("PCK_ABMS_BANCO_GENTE.PR_ABM_AREAS")
                .AddParam(area.Id)
                .AddParam(area.Nombre)
                .AddParam(area.Descripcion)
                .AddParam(default(long?))
                .AddParam(area.UsuarioUltimaModificacion.Id)
                .JustExecute();
        }

        public IList<AreaResultado.Consulta> ConsultarAreasCombo()
        {
            return Execute("PR_OBTENER_TABLAS_SATELITES")
                .AddParam("T_AREAS")
                .ToListResult<AreaResultado.Consulta>();
        }
    }
}