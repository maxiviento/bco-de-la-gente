using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http.Dependencies;
using Ninject;
using Ninject.Modules;
using Ninject.Web.WebApi;

namespace Infraestructura.Core.DI
{
    public class NinjectHttpResolver : IDependencyResolver, IDependencyScope
    {
        public IKernel Kernel { get; private set; }
        public NinjectHttpResolver(params NinjectModule[] modules)
        { 
            Kernel = new StandardKernel(modules);
        }

        public NinjectHttpResolver(Assembly assembly)
        {
            Kernel = new StandardKernel();
            Kernel.Load(assembly);
        }

        public object GetService(Type serviceType)
        {
            return Kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Kernel.GetAll(serviceType);
        }

        public void Dispose()
        {
            //Do Nothing
        }

        public IDependencyScope BeginScope()
        {
            return new NinjectDependencyScope(Kernel.BeginBlock());
        }
    }
}
