using System.Web.Http;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;

namespace Infraestructura.Core.DI
{
    public class Injector
    {
        private static NinjectHttpResolver _resolver;

        //Register Ninject Modules
        public static void Registrar(HttpConfiguration config)
        {
            _resolver = new NinjectHttpResolver(NinjectHttpModules.Modulos);
            GlobalConfiguration.Configuration.DependencyResolver = _resolver;
            config.DependencyResolver = _resolver;
        }
        public static void Registrar()
        {
            _resolver = new NinjectHttpResolver(NinjectHttpModules.Modulos);
            GlobalConfiguration.Configuration.DependencyResolver = _resolver;
        }

        public static T GetService<T>()
        {
            return (T) _resolver.GetService(typeof(T));
        }

        public static IKernel GetCurrentKernel()
        {
            return _resolver.Kernel;
        }
    }
}