using System.Web.Http.Controllers;
using System.Web.Http.ValueProviders;
using IValueProvider = System.Web.Http.ValueProviders.IValueProvider;

namespace Infraestructura.Core.Proveedores
{
    
    public class CustomBindingValueProviderFactory : ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(HttpActionContext actionContext)
        {
            return new CustomBindingValueProvider(actionContext);
        }
    }
}
