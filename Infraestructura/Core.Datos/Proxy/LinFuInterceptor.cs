using LinFu.DynamicProxy;

namespace Infraestructura.Core.Datos.Proxy
{
   public class LinFuInterceptor: IInterceptor
    {
        public object Intercept(InvocationInfo info)
        {
            return info.Target;
            
        }
    }
}
