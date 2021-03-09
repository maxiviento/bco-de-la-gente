using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;
using Infraestructura.Core.Datos.DSL;
using NHibernate;

namespace Infraestructura.Core.Datos
{
    public abstract class NhRepositorio<TEntidad> : IRepositorio<TEntidad> where TEntidad : Entidad
    {
        public ISession Sesion { get; private set; }
        public IStatelessSession StatelessSession { get; private set; }

        protected NhRepositorio(ISession sesion)
        {
            Sesion = sesion;
        }

        protected NhRepositorio(ISession sesion, IStatelessSession statelessSession)
        {
            Sesion = sesion;
            StatelessSession = statelessSession;
        }


        public StoreProcedure Execute(string spName)
        {
            return new StoreProcedure(string.Format("{0}{1}", GetOwnerName(), spName), Sesion);
        }

        public T InsertarOActualizar<T>(string spName, T entidad) where T : Entidad
        {
            var spResutl = Execute(spName).AddParamFromQuery(entidad).ToSpResult();
            entidad.Id = spResutl.Id;
            return entidad;
        }

        private string GetOwnerName()
        {
            var ownerName = ConfigurationManager.AppSettings["db:owner"];
            if (!string.IsNullOrEmpty(ownerName))
            {
                ownerName = string.Format("{0}.", ownerName);
            }
            return ownerName;
        }

        public T ConsultarPorId<T>(string spName, Id id) where T : Entidad
        {
            return Execute(spName).AddParam(id).ToUniqueResult<T>();
        }

        public IList<T> ObtenerTodos<T>(string tableName) where T : class
        {
            return Execute("PR_OBTENER_TABLAS_SATELITES")
                .AddParam(tableName)
                .ToListResult<T>();
        }

        public Resultado<R> ConsultarPor<C, R>(string spName, C c)
            where C : Consulta
            where R : class
        {
            c.TamañoPagina++;

            var elementosEncontrados = Execute(spName)
                .AddParamFromQuery(c)
                .ToListResult<R>()
                .ToList();
            var resultado = new Resultado<R>()
            {
                Elementos = elementosEncontrados,
                NumeroPagina = c.NumeroPagina,
                TamañoDePagina = c.TamañoPagina - 1,
                TieneMasResultados = elementosEncontrados.Count > c.TamañoPagina - 1
            };

            return resultado;
        }

        public Resultado<R> CrearResultado<C, R>(C consulta, IList<R> elementos)
            where C : Consulta
            where R : class
        {
            var tamañoPagina = consulta.PaginaDesde == 0 ? consulta.TamañoPagina - 1 : consulta.TamañoPagina;
            var resultado = new Resultado<R>()
            {
                Elementos = elementos.Take(consulta.TamañoPagina - 1),
                NumeroPagina = consulta.NumeroPagina,
                TamañoDePagina = consulta.TamañoPagina,
                TieneMasResultados = elementos.Count > consulta.TamañoPagina - 1
            };

            if (resultado.TieneMasResultados)
                resultado.Elementos = resultado.Elementos.Take(resultado.TamañoDePagina).ToList();

            return resultado;
        }
        public StoreProcedureStateless RunStateless(string spName)
        {
            var ownerName = ConfigurationManager.AppSettings["db:owner"];
            if (!string.IsNullOrEmpty(ownerName))
            {
                ownerName = $"{ownerName}.";
            }

            return new StoreProcedureStateless($"{ownerName}{spName}", StatelessSession);
        }

        /// <summary>
        /// Recomendado usar el metodo <see cref="CrearResultado{C,R}"/>
        /// </summary>
        /// <typeparam name="C"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="elementos"></param>
        /// <param name="consulta"></param>
        /// <returns></returns>
        public Resultado<R> ArmarResultado<C, R>(IList<R> elementos, C consulta)
            where C : Consulta
            where R : class
        {
            var resultado = new Resultado<R>()
            {
                Elementos = elementos,
                NumeroPagina = consulta.NumeroPagina,
                TamañoDePagina = consulta.TamañoPagina,
                TieneMasResultados = elementos.Count > consulta.TamañoPagina
            };

            if (resultado.TieneMasResultados)
                resultado.Elementos = resultado.Elementos.Take(resultado.TamañoDePagina).ToList();

            return resultado;
        }
    }
}