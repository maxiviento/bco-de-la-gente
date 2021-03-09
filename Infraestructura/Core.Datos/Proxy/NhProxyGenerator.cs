using System.ComponentModel;
using Castle.DynamicProxy;
using NHibernate.Proxy.DynamicProxy;

namespace Infraestructura.Core.Datos.Proxy
{
    public class NhProxyGenerator
    {
        private static ProxyGenerator _proxyGenerator;


        private NhProxyGenerator()
        {
        }

        private static ProxyGenerator GetGenerator()
        {
            if (_proxyGenerator == null)
                _proxyGenerator = new ProxyGenerator();
            return _proxyGenerator;
        }

        public static T CreateProxy<T>(T t) where T : class
        {
            
           var proxy = GetGenerator()
                         .CreateClassProxyWithTarget<T>(t,
                            new DirtyObjectInterceptor<T>(t));

            return proxy;
        }

        public static T CreateProxy<T>() where T : class
        {
            var proxy = GetGenerator()
                           .CreateClassProxy<T>
                           (new DirtyObjectInterceptor<T>());
            return proxy;
        }

        public static T CreateNHProxy<T>(T target)
        {
            var factory = new ProxyFactory();
            
            var proxy = factory.CreateProxy(typeof(T), new NotifyPropertyChangedProxyInterceptor(target),
                typeof(INotifyPropertyChanged));
            return (T) proxy;
        }

        public static T CreateProxyLinFu<T>(T target)
        {
            var proxyFactory = new LinFu.DynamicProxy.ProxyFactory();
            var proxy = proxyFactory.CreateProxy<T>(new LinFuInterceptor());
            return proxy;
        }
    }
}