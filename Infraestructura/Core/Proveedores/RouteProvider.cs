using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace Infraestructura.Core.Proveedores
{
    public class RouteProvider : DefaultDirectRouteProvider
    {
        protected override string GetRoutePrefix(HttpControllerDescriptor descriptor)
        {
            var routePrefix =  base.GetRoutePrefix(descriptor) ?? descriptor.ControllerName;

#if DEBUG
            if (!routePrefix.StartsWith("api"))
            {
                routePrefix = string.Format("api/{0}", routePrefix);
            }
#endif

            return routePrefix;
        }

      
    }
}
