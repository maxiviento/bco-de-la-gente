using System;
using System.Collections;
using System.Web.Http;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.DI;
using Soporte.Dominio.Modelo;

namespace Soporte.Aplicacion.Servicios
{
    public class ParametrosSingleton
    {
        private readonly ParametrosServicio _parametrosServicio;
        protected static ParametrosSingleton instance ;
        protected static readonly object padlock = new object();
        private Hashtable data;

        private static NinjectHttpResolver Resolver;
        
        private ParametrosSingleton()
        {
            Resolver = (NinjectHttpResolver) GlobalConfiguration.Configuration.DependencyResolver;
            _parametrosServicio = (ParametrosServicio )Resolver.GetService(typeof(ParametrosServicio));
            data = new Hashtable();
        }
        
        /// Obtener la instancia unica (Singleton) 
        public static ParametrosSingleton Instance
        {
            get
            { 
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new ParametrosSingleton();
                        }
                    }
                }

                return instance;
            }
        }

        /// Recupera un valor.
        public string this[string key]
        {
            get
            {
                return this.GetValue(key);
            }
        }
        
        /// Recupera un valor de un repositorio de configuracion.
        protected virtual string GetDataFromConfigRepository(string key)
        {
            return _parametrosServicio.ObtenerValorVigenciaParametroPorFecha(new Id(key), DateTime.Today).Valor;
        }
        
        private void StoreDataInCache(string key, object val)
        {
            lock (instance.data.SyncRoot)
            {
                // si el elemento ya esta en la lista de datos... 
                if (instance.data.ContainsKey(key))
                {
                    // lo quito 
                    instance.data.Remove(key);
                }

                // y lo vuelvo a añadir 
                instance.data.Add(key, val);
            }
        }
        
        /// Retorna un valor de la coleccion interna de datos.
        public string GetValue(string key)
        {
            string ret;

            // si el dato esta en el cache...
            if (instance.data.ContainsKey(key) && GetParametro(key).FechaAlmacenado.Equals(DateTime.Today))
            {
                ret = GetParametro(key).Valor;
            }
            else // si no esta en el cache
            {
                // recupero el parametro del repositorio
                ret = this.GetDataFromConfigRepository(key);

                // si lo encontro, lo almaceno en el cache
                if (ret != null)
                    StoreDataInCache(key, new ParametroCache { Valor= ret, FechaAlmacenado = DateTime.Today});
            }   
            return ret;
        }
        
        /// Retorna true si contiene la clave especificada.
        public bool Contains(string key)
        {
            return instance.data.ContainsKey(key);
        }
        
        /// Retorna el objeto ParametroCache si contiene la clave especificada. 
        /// Retorna null en caso que no se encuentre.
        private ParametroCache GetParametro(string key)
        {
            return instance.data.ContainsKey(key) ? (ParametroCache) instance.data[key] : null;
        }
        
        /// Limpia los datos.
        public void Clear()
        {
            lock (instance.data.SyncRoot)
            {
                instance.data.Clear();
            }
        }
    }
}
