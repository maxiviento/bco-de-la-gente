using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Datos.Proxy;
using NUnit.Framework;

namespace Core.Tests
{
    [TestFixture]
    public class ProxyDinamicoTest
    {
        [Test]
        public void CrearProxyTest()
        {

            var usuario = new Usuario();
            usuario.Cuil = "el gran cuil";

            var proxy = NhProxyGenerator.CreateProxy(usuario);
            proxy.Cuil = "el gran cuil";
            proxy.Cuil = "el gran cuil x";

            if (ProxyUtil.IsProxy(proxy))
            {
                var objectProxed = proxy as IProxyTargetAccessor;
                var intercetpor = (DirtyObjectInterceptor<Usuario>) objectProxed.GetInterceptors()[0];
                var parameters = intercetpor.GetParameters();
            }
        }

        [Test]
        public void CrearProxyTestLunFu()
        {

            var usuario = new Usuario();
            usuario.Cuil = "el gran cuil";

            var proxy = NhProxyGenerator.CreateProxyLinFu(usuario);
            proxy.Cuil = "el gran cuil";
            proxy.Cuil = "el gran cuil x";

            if (ProxyUtil.IsProxy(proxy))
            {
                var objectProxed = proxy as IProxyTargetAccessor;
                var intercetpor = (DirtyObjectInterceptor<Usuario>)objectProxed.GetInterceptors()[0];
                var parameters = intercetpor.GetParameters();
            }
        }
    }
}
